
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
    :sort-options="{
      enabled: true,
      initialSortBy: { field: 'id', type: 'asc' },
    }"
    :search-options="{
      enabled: true,
    }"
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
      <div class="d-flex" v-if="props.column.field == 'edit'">
        <button
          type="button"
          class="btn btn-primary"
          style="width: 4em; height: 2.4em"
          @click="ceva(props.row.id)"
        >
          <i class="bi bi-printer"></i>
        </button>
        &nbsp;
      </div>
    </template>
  </vue-good-table>
</template>

<script>
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

const token =
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiMSIsInJvbGUiOiJVc2VyIiwibmJmIjoxNjk0MDg2ODM1LCJleHAiOjE2OTQ2OTE2MzUsImlhdCI6MTY5NDA4NjgzNSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzE2NCIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcxNjQifQ.buxKyk5Tv_o5J3syVWZuziFJ46ZXCxnp2G6XV9yPJ3g";
apiInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;

export default {
  components: {
    VueGoodTable,
  },
  data() {
    return {
      ids: [],
      columns: [
        {
          label: "Delivery note no.:",
          field: "id",
          type: "number",
          width: "5%",
        },
        {
          label: "Bag Numbers",
          field: "bagNumbers",
          //type: "string",
        },
        {
          label: "",
          field: "edit",
          sortable: false,
        },
      ],
      rows: [],
    };
  },
  mounted() {
    fetch("https://localhost:7164/DeliveryNotePrinted")
      .then((res) => {
        if (res.ok) {
          return res.json();
        }
      })
      .then((data) => (this.rows = data))
      .catch((err) => {
        $toast.error("Error fetching data") + err;
      });
  },
  methods: {
    ceva(id) {
      const reportId = id;
      console.log(id);
      // Salvați id-ul raportului în local storage
      localStorage.setItem("reportId", reportId);

      // Redirecționați utilizatorul la pagina /PastReport
      window.location.href = "/PastReport";
    },
  },
};
</script>

<style>
@import url("https://unpkg.com/css.gg@2.0.0/icons/css/trash.css");
@import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.3/font/bootstrap-icons.css");
.btn-report {
  background: #008cba;
  color: white;
  text-align: center;
  border-radius: 500px;
  margin: 10px auto;
}
.btn-succes {
  background: #008cba;
  color: white;
  text-align: center;
  border-radius: 500px;
  margin: 10px auto;
}
</style>