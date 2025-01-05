<template>
  <div id="app">
    <div id="printable-content" class="to-deliver">
      <div class="container">
        <div class="col-md-6 d-flex justify-content-between">
          <h2 id="deliveryNumber" style="font-size: 16px">
            Delivery Number: {{ deliveryNumber }}
          </h2>
          <div class="printOnly">
            <div style="display: flex; align-items: center">
              <div style="flex: 1">
                MRA49B, Marsa Industrial Estate,<br />
                Marsa.<br />
                VAT 17434222
              </div>
              <div style="flex: 0">
                <img
                  src="../assets/inspectra-logo-dark.png"
                  alt="Inspectra Logo"
                  class="img-fluid"
                  style="max-width: 100px"
                />
              </div>
            </div>
          </div>
        </div>
      </div>
      <br />
      <br />
      <vue-good-table
        :columns="columns"
        :rows="rows"
        styleClass="vgt-table condensed"
      >
      </vue-good-table>
      <div class="printOnly">
        <div class="footer-text">Delivered on time By</div>
        <div class="footer-text">Checked by</div>
        <div class="footer-text">Received on time By</div>
      </div>
    </div>
    <div class="col-md-6 d-flex justify-content-between">
      <button class="btn-save" @click="printContent">Print page</button>

      <button class="btn-save" @click="exportToCSV">Export CSV</button>
    </div>
  </div>
</template>
    
    <script >
import { VueGoodTable } from "vue-good-table-next";
import axios from "axios";
export const apiInstance = axios.create({
  baseURL: "https://localhost:7164",
  responseType: "json",
  headers: {
    //'Content-Type': 'application/json',
    // you dont need to pre-define all headers
  },
});
import { token } from "./token.js";
apiInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;

export default {
  el: "#app",
  components: {
    VueGoodTable,
  },
  data() {
    const storedData = localStorage.getItem("selectedRowsData");
    let parsedData = [];

    if (storedData) {
      parsedData = JSON.parse(storedData);
      //console.log(parsedData);
      const ids = parsedData.map((item) => item.id);
      console.log(ids);
    }

    return {
      columns: [
        {
          label: "Bag Number",
          field: "bagNumber",
        },
        {
          label: "Chit Number",
          field: "chitNumber",
        },
        {
          label: "MO Number",
          field: "moNumber",
        },
        {
          label: "Part Number",
          field: "partNumber",
        },
        {
          label: "Quantity Issued",
          field: "quantityIssued",
          type: "number",
        },
        {
          label: "Quantity Passed",
          field: "quantityPassed",
          type: "number",
        },
        {
          label: "Quantity Reject",
          field: "quantityFailed",
          type: "number",
        },
      ],
      rows: parsedData,
      codes: [],
      users: [],
    };
  },
  methods: {
    exportToCSV() {
      const csvHeader = this.columns.map((column) => column.label).join(",");
      const csvData = this.rows.map((row) =>
        this.columns.map((column) => row[column.field]).join(",")
      );
      const csvContent = [csvHeader, ...csvData].join("\n");
      const blob = new Blob([csvContent], { type: "text/csv" });
      const link = document.createElement("a");
      link.href = URL.createObjectURL(blob);
      link.download = "table_data.csv";
      link.style.display = "none";
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    },
    printContent() {
      localStorage.clear();
      const printContents =
        `<br>` +
        `<div style="font-weight: bold; font-size: 25px;">Despatched to Trelleborg</div>` +
        document.getElementById("printable-content").innerHTML;
      const originalContents = document.body.innerHTML;

      document.body.innerHTML = printContents;
      window.print();
      document.body.innerHTML = originalContents;
      window.location.reload();
    },
  },
  mounted() {
    const reportId = localStorage.getItem("reportId");

    if (!reportId) {
      console.error("reportId not found in local storage.");
      return;
    }

    // Actualizați URL-ul pentru a include reportId
    const url = `https://localhost:7164/DeliveryNotePrinted/GetDeliveryNotes/${reportId}`;

    fetch(url)
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else {
          throw new Error("Error selecting data");
        }
      })
      .then((data) => {
        this.rows = data;
        this.deliveryNumber = reportId;
        localStorage.setItem("selectedRowsData", JSON.stringify(data));
        localStorage.clear;
      })
      .catch((error) => {
        console.error(error);
      });
  },
};
</script>
    
    <style scoped>
.btn-save {
  margin-top: 20px;
  background-color: #4caf50;
  color: white;
  padding: 10px 20px;
  border: none;
  cursor: pointer;
  border-radius: 5px;
}

.printOnly {
  display: none;
}
.green-border-input {
  border: 2px solid green;
  border-radius: 5px;
  padding: 5px; /* Spațiu interior */
}
@media screen {
  /* Stilurile pentru ecranul standard aici */

  /* De exemplu, puteți seta dimensiunea fontului pentru tabel */
  .vgt-table td {
    font-size: 10px; /* Ajustați dimensiunea fontului după cum este necesar */
  }
}
@media print {
  body .vgt-table important {
    font-size: 12px; /* Ajustați dimensiunea fontului pentru a se încadra */
    /* Alte stiluri specifice pentru tipărire aici */
  }
  .printOnly {
    display: block;
  }
  .footer-text {
    flex: 1; /* Lățimea fiecărui chenar */
    text-align: left;
    justify-content: space-between;
    margin-top: 20px; /* Spațiu de sus */
    padding: 10px; /* Spațiu în interiorul chenarelor */
    border: 1px solid #000; /* Linie de contur a chenarului */
  }

  vgt-text {
    font-size: 10px; /* Ajustați dimensiunea fontului pentru a se încadra pe o pagină A4 */
  }
}
</style>
    