
<template>
  <div class="mx-4 text-center py-3" style="color: #008cba">
    <h1>Employees Table</h1>
  </div>
  <div class="container-fluid">
    <div class="col-5 col-lg-2 offset-lg-3 col-md-3 offset-md-2">
      <button
        type="button"
        class="btn btn-success mb-3"
        data-bs-toggle="modal"
        data-bs-target="#createEmployee"
      >
        New Employee
      </button>
    </div>
    <div
      class="modal fade mt-5"
      id="createEmployee"
      tabindex="-1"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5">Add Employee</h1>
            <button
              type="button"
              class="btn-close"
              data-bs-dismiss="modal"
              aria-label="Close"
            ></button>
          </div>
          <div class="modal-body">
            <form>
              <div class="form-group">
                <label for="name">Name</label>
                <input
                  type="text"
                  class="form-control"
                  id="first_name"
                  v-model="name"
                  placeholder="Enter name"
                />
                <label for="surname" class="mt-2">Surname</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="surname"
                  id="surname"
                  placeholder="Surname"
                />
                <label for="punchCode" class="mt-2">Punch Code</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="punchCode"
                  id="punchCode"
                  placeholder="punchCode"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer d-flex justify-content-center">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-primary"
              @click="addUser()"
            >
              Create
            </button>
          </div>
        </div>
      </div>
    </div>

    <vue-good-table
      :columns="columns"
      :rows="rows"
      max-height="56vh"
      class="col-lg-6 offset-lg-3 col-md-8 offset-md-2 col-12 me-1"
      :sort-options="{
        enabled: true,
        initialSortBy: { field: 'id', type: 'asc' },
      }"
      :search-options="{
        enabled: true,
      }"
      :pagination-options="{
        enabled: true,
        perPage: 10,
        perPageDropdown: [5, 10, 20, 25],
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
            style="width: 2.4em; height: 2.4em"
            data-bs-toggle="modal"
            :data-bs-target="'#' + props.row.id"
          >
            <i class="bi bi-pencil"></i>
          </button>
          &nbsp;
          <button
            type="button"
            class="btn btn-danger"
            v-on:click="showAlert(props.row.id)"
          >
            <i class="gg-trash"></i>
          </button>
          <div
            class="modal fade mt-5"
            :id="props.row.id"
            tabindex="-1"
            aria-hidden="true"
          >
            <div class="modal-dialog">
              <div class="modal-content">
                <div class="modal-header">
                  <h1 class="modal-title fs-5">
                    Edit {{ props.row.name }} {{ props.row.surname }}
                    {{ props.row.punchCode }}
                  </h1>
                  <button
                    type="button"
                    class="btn-close"
                    data-bs-dismiss="modal"
                    aria-label="Close"
                  ></button>
                </div>
                <div class="modal-body">
                  <form>
                    <div class="form-group">
                      <label for="name">Name</label>
                      <input
                        type="text"
                        class="form-control"
                        id="first_name"
                        v-model="props.row.name"
                        placeholder="Enter name"
                      />
                      <label for="surname" class="mt-2">Surname</label>
                      <input
                        type="text"
                        class="form-control"
                        v-model="props.row.surname"
                        id="surname"
                        placeholder="Surname"
                      />
                      <label for="punchCode" class="mt-2">Punch Code</label>
                      <input
                        type="text"
                        class="form-control"
                        v-model="punchCode"
                        id="punchCode"
                        placeholder="punchCode"
                      />
                    </div>
                  </form>
                </div>
                <div class="modal-footer d-flex justify-content-center">
                  <button
                    type="button"
                    data-bs-dismiss="modal"
                    class="btn btn-primary"
                    @click="
                      editUser(
                        props.row.id,
                        props.row.name,
                        props.row.surname,
                        props.row.punchCode
                      )
                    "
                  >
                    Edit
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      </template>
    </vue-good-table>
  </div>
</template>

<script>
export const apiInstance = axios.create({
  baseURL: "https://localhost:7164",
  responseType: "json",
  headers: {
    //Content-Type: 'CONTENT_TYPE',
    // you dont need to pre-define all headers
  },
});
import { token } from "./token.js";
apiInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;
import { useToast } from "vue-toast-notification";
const $toast = useToast();
import "vue-good-table-next/dist/vue-good-table-next.css";
import { VueGoodTable } from "vue-good-table-next";
import axios from "axios";
export default {
  name: "EmployeeTable",
  components: {
    VueGoodTable,
  },
  data() {
    return {
      selected: null,
      options: ["test1", "test2", "test3"],
      name: "",
      surname: "",
      goods: [],
      columns: [
        {
          label: "id",
          field: "id",
          type: "number",
          width: "5%",
        },
        {
          label: "Name",
          field: "name",
          width: "15%%",
        },
        {
          label: "Surname",
          field: "surname",
          width: "15%%",
        },
        {
          label: "Punch Code",
          field: "punchCode",
          width: "15%%",
        },
        {
          label: "",
          field: "edit",
          sortable: false,
          width: "8%",
        },
      ],
      rows: [],
    };
  },
  methods: {
    disablebag() {
      return true;
    },

    showAlert(id) {
      this.$swal
        .fire({
          title: "Are you sure?",
          text: "You won't be able to revert this!",
          showCancelButton: true,
          cancelButtonColor: "#d33",
          confirmButtonColor: "#3085d6",
          confirmButtonText: "Yes, delete it!",
          reverseButtons: true,
        })
        .then((result) => {
          if (result.isConfirmed) {
            this.remove(id);
          }
        });
    },

    remove(id) {
      apiInstance;
      fetch(`https://localhost:7164/Employee/${id}`, { method: "DELETE" })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(`Employee ${id} deleted`);
          }
        })
        .catch((err) => $toast.error("Error deleting Employee ") + err);
    },
    editUser(id, name, surname) {
      apiInstance
        .put(`https://localhost:7164/Employee/${id}`, {
          name: name,
          surname: surname,
          punchCode: this.punchCode,
        })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(
              `Employee ${this.name} ${this.surname}(${this.punchCode}) edited`
            );
          }
        })
        .catch((err) => $toast.error("Error editing Employee") + err);
    },

    addUser() {
      apiInstance
        .post(`https://localhost:7164/Employee`, {
          name: this.name,
          surname: this.surname,
          punchCode: this.punchCode,
        })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();

            $toast.success(
              `Employee ${this.name} ${this.surname}(${this.punchCode}) created`
            );
          }
        })
        .catch((err) => $toast.error("Error creating Employee") + err);
    },
    refreshData() {
      fetch("https://localhost:7164/Employee")
        .then((res) => {
          if (res.ok) {
            return res.json();
          } else $toast.error("Error fetching Employees");
        })
        .catch((err) => {
          $toast.error(err);
        })
        .then((data) => (this.rows = data));
    },
  },
  mounted() {
    fetch("https://localhost:7164/Employee")
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else $toast.error("Error fetching Employees");
      })
      .then((data) => (this.rows = data))
      .catch((err) => {
        $toast.error("Error fetching Employees") + err;
      });
  },
};
</script>



<style scoped>
@import url("https://unpkg.com/css.gg@2.0.0/icons/css/trash.css");
@import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.3/font/bootstrap-icons.css");
</style>
<style src="vue-multiselect/dist/vue-multiselect.css"></style>