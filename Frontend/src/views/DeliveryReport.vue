<template>
  <div class="mx-4 text-center py-3" style="color: #008cba">
    <router-link to="/" class="text-decoration-none btn btn-success mx-2"
      >Issued</router-link
    >
    <router-link
      to="/returned"
      class="btn btn-success text-decoration-none mx-2"
      >Received</router-link
    >
    <router-link
      :to="`/DeliveryNote/Valid`"
      class="btn btn-success text-decoration-none mx-2"
      >Dispatched</router-link
    >
    <router-link
      :to="`/QualifiersTab`"
      class="btn btn-success text-decoration-none mx-2"
      >Qualifiers</router-link
    >
    <router-link
      :to="`/InStock`"
      class="btn btn-success text-decoration-none mx-2"
      >Stock</router-link
    >
    <router-link
      :to="`/PastDelivery`"
      class="btn btn-success text-decoration-none mx-2"
      >Past Delivery Notes</router-link
    >
    <router-link
      :to="`/EmployeeHours`"
      class="btn btn-success text-decoration-none mx-2"
      >Salary</router-link
    >
  </div>
  <vue-good-table
    :columns="columns"
    :rows="rows"
    max-height="70vh"
    class="col-12 me-1 mx-lg-0 mx-md-0"
    v-on:selected-rows-change="handleSelectedRows"
    :sort-options="{
      enabled: true,
      initialSortBy: { field: 'id', type: 'asc' },
    }"
    :search-options="{ enabled: true }"
    :select-options="{ enabled: true }"
    :pagination-options="{
      enabled: true,
      perPage: 20,
      perPageDropdown: [5, 10, 20, 25, 50],
      dropdownAllowAll: true,
      nextLabel: 'next',
      prevLabel: 'prev',
      rowsPerPageLabel: 'Rows per page',
      ofLabel: 'of',
      pageLabel: 'page', // for 'pages' mode
      allLabel: 'All',
    }"
  >
    <template #table-row="props">
      <div
        :rowSelection="rowSelection"
        class="d-flex"
        v-if="props.column.field == 'edit'"
      ></div>
    </template>
    <template #selected-row-actions>
      <router-link class="btn-report" to="/toDeliver" @click="deliverNo">
        Generate Delivery Report
      </router-link>
    </template>
  </vue-good-table>
  <button class="btn btn-primary" @click="exportToCSV">Export CSV</button>
</template>

<script>
//import axios from "axios";
import { useToast } from "vue-toast-notification";
const $toast = useToast();
import "vue-good-table-next/dist/vue-good-table-next.css";
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
  props: ["rowSelection"],

  name: "GoodsTable",

  components: {
    VueGoodTable,
  },

  data() {
    return {
      ids: [],
      columns: [
        {
          label: "id",
          field: "id",
          type: "number",
          width: "5%",
        },
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
        {
          label: "Notes",
          field: "notes",
        },
      ],
      rows: [],
      codes: [],
      users: [],

      goodsDelivery: "",
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

    handleSelectedRows(rows) {
      const ids = [];
      const bagNumbers = [];
      for (const row of rows.selectedRows) {
        ids.push(row.id);
        bagNumbers.push(row.bagNumber);
      }
      this.ids = ids;
      this.bagNumbers = bagNumbers;
      const queryParams = ids.map((id) => `ids=${id}`).join("&");
      const url = `https://localhost:7164/DeliveryNote/Valid/GetSelectedRows?${queryParams}`;

      fetch(url)
        .then((res) => {
          if (res.ok) {
            return res.json();
          } else {
            throw new Error("Error selecting data");
          }
        })
        .then((data) => {
          localStorage.setItem("selectedRowsData", JSON.stringify(data));
        })
        .catch((error) => {
          console.error(error);
        });
    },
    deliverNo() {
      apiInstance
        .post(
          `https://localhost:7164/DeliveryNotePrinted/CreateDeliveryNotePrinted`,
          {
            deliveryNoteIds: this.ids,
            bagNumbers: this.bagNumbers,
          }
        )
        .then((res) => {
          if (res.status >= 200 && res.status <= 205) {
            console.log("IDs successfully sent to the new API.");
            localStorage.clear();
          } else {
            throw new Error("Error sending data to new API");
          }
        })
        .catch((error) => {
          console.error(error);
        });
    },

    refreshData() {
      if (this.$router && this.$router.params && this.$router.params.id) {
        this.id = this.$router.params.id;
        fetch(`https://localhost:7164/DeliveryNote/Valid`)
          .then((res) => {
            if (res.ok) {
              return res.json();
            } else {
              throw new Error("Error fetching Data");
            }
          })
          .then((data) => {
            this.rows = data;
          })
          .catch((err) => {
            $toast.error("Error fetching data: " + err);
          });
      } else {
        $toast.error("Invalid URL: 'id' parameter is missing.");
      }
    },
  },

  mounted() {
    fetch("https://localhost:7164/DeliveryNote/Valid")
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else $toast.error("Error fetching Data");
      })
      .then((data) => (this.rows = data))
      .catch((err) => {
        $toast.error("Error fetching data" + err);
      }),
      fetch("https://localhost:7164/Employee")
        .then((res) => {
          if (res.ok) {
            return res.json();
          } else $toast.error("Error fetching Employees");
        })
        .then((data) => (this.users = data))
        .catch((err) => {
          $toast.error("Error fetching Employees" + err);
        });
  },
};
</script>

<style scoped>
@import url("https://unpkg.com/css.gg@2.0.0/icons/css/trash.css");
@import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.3/font/bootstrap-icons.css");
.btn-report {
  background: #008cba;
  color: white;
  text-align: center;
  border-radius: 500px;
  margin: 10px auto;
}
</style>