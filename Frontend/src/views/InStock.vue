<template>
  <div
    class="mx-4 text-center py-3"
    style="color: #008cba"
    @click="refreshData"
  >
    <router-link to="/" class="text-decoration-none btn btn-success mx-2"
      >Issued</router-link
    >
    <router-link
      to="/returned"
      class="btn btn-success text-decoration-none mx-2"
      >Received</router-link
    >
    <router-link
      to="/DeliveryNote/Valid"
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
      pageLabel: 'page',
      allLabel: 'All',
    }"
  >
    <span v-if="props.column.field == 'assignedTo'">
      {{ users.find((user) => user.id === props.row.assignedTo).name }}
      {{ users.find((user) => user.id === props.row.assignedTo).surname }}
    </span>
    <template #table-row="props">
      <div
        :rowSelection="rowSelection"
        class="d-flex"
        v-if="props.column.field == 'edit'"
      ></div>
    </template>
    <!-- <div v-if="props.column.field == 'assignedTo'">
      {{ getEmployeeName(props.row.assignedTo) }}
      <div v-for="user in users" :key="user.id" :value="user.id">
        {{ user.punchCode }}({{ user.name }} {{ user.surname }})
      </div>
    </div> -->
  </vue-good-table>
  <button class="btn-save" @click="exportToCSV">Export CSV</button>
</template>

<script>
export const apiInstance = axios.create({
  baseURL: "https://localhost:7164",
  responseType: "json",
  headers: {
    //'Content-Type': 'application/json',
    // you dont need to pre-define all headers
  },
});
const token =
  "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMSIsInJvbGUiOiJVc2VyIiwibmJmIjoxNjk0MDg2ODM1LCJleHAiOjE2OTQ2OTE2MzUsImlhdCI6MTY5NDA4NjgzNSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE2NCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcxNjQifQ.buxKyk5Tv_o5J3syVWZuziFJ46ZXCxnp2G6XV9yPJ3g";
apiInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;

import axios from "axios";
import { useToast } from "vue-toast-notification";
const $toast = useToast();
import "vue-good-table-next/dist/vue-good-table-next.css";
import { VueGoodTable } from "vue-good-table-next";
export default {
  name: "GoodsTable",
  components: {
    VueGoodTable,
  },
  data() {
    return {
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
          label: "MO Number",
          field: "moNumber",
        },
        {
          label: "Chit Number",
          field: "chitNumber",
        },
        {
          label: "Part Number",
          field: "partNumber",
        },
        {
          label: "Supplier",
          field: "supplier",
        },
        {
          label: "Inspection Type",
          field: "inspectionType",
        },
        {
          label: "Quality",
          field: "quality",
        },

        {
          label: "Quantity Issued",
          field: "quantityIssued",
          type: "number",
        },
        {
          label: "Date Issued",
          field: "dateIssued",
          type: "string",
        },
        {
          label: "Requested Date",
          field: "requestedDate",

          inputFormat: "DD/MM/YYYY",
          outputFormat: "DD/MM/YYYY",
        },
        {
          label: "Assigned To",
          field: "assignedTo",
        },
        {
          label: "Assigned Date",
          field: "assignedDate",
        },
        {
          label: "Is Checked",
          field: "isChecked",
        },
        {
          label: "Is Returned",
          field: "isReturned",
        },
      ],
      rows: [],
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
    getEmployeeName(userId) {
      console.log(userId);
      const user = this.users.find((u) => u.id === userId);
      if (user) {
        return `${user.punchCode} (${user.name} ${user.surname})`;
      }
      return ""; // Sau puteți gestiona cazul în care utilizatorul nu este găsit
    },
  },
  mounted() {
    fetch("https://localhost:7164/BagInventory/GetWhatIsInStock")
      .then((res) => {
        if (res.ok) {
          return res.json();
        }
      })
      .then((data) => {
        this.rows = data.map((item) => ({
          ...item,
          isChecked: item.isChecked ? "Yes" : "No",
          isReturned: item.isReturned ? "Yes" : "No",
        }));
      })
      .catch((err) => {
        $toast.error("Error fetching data") + err;
      }),
      fetch("https://localhost:7164/Employee")
        .then((res) => {
          if (res.ok) {
            return res.json();
          } else $toast.error("Error fetching Employees");
        })
        .then((data) => (this.users = data))
        .catch((err) => {
          $toast.error("Error fetching Employees") + err;
        });
  },
};
</script>

<style>
.btn-save {
  margin-top: 20px;
  background-color: #4caf50;
  color: white;
  padding: 10px 20px;
  border: none;
  cursor: pointer;
  border-radius: 5px;
}
</style>