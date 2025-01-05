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
  <div class="container-fluid">
    <div class="col-5 col-lg-2 col-md-3">
      <button
        type="button"
        class="btn btn-success mb-3"
        data-bs-toggle="modal"
        data-bs-target="#createQualifiers"
      >
        New
      </button>
    </div>
    <div
      class="modal fade mt-5"
      id="createQualifiers"
      tabindex="-1"
      aria-hidden="true"
    >
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h1 class="modal-title fs-5">Add</h1>
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
                <label for="responsiblePerson">Responsible Person</label>
                <input
                  type="text"
                  class="form-control"
                  id="responsiblePerson"
                  v-model="name"
                  placeholder="Enter Responsible Person"
                />
                <label for="dateOfIssue" class="mt-2">Date of Issue</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="dateOfIssue"
                  id="dateOfIssue"
                  placeholder="Date of Issue"
                />
                <label for="partNumber" class="mt-2">Part Number</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="partNumber"
                  id="partNumber"
                  placeholder="partNumber"
                />
                <label for="type" class="mt-2">Type</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="type"
                  id="type"
                  placeholder="type"
                />
                <label for="tssCode" class="mt-2">TSS Code</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="tssCode"
                  id="tssCode"
                  placeholder="tssCode"
                />
                <label for="chargePerPiece" class="mt-2">chargePerPiece</label>
                <input
                  type="text"
                  class="form-control"
                  v-model="chargePerPiece"
                  id="chargePerPiece"
                  placeholder="chargePerPiece"
                />
                <label for="payPerPiece" class="mt-2">Pay Per Piece</label>
                <input
                  type="number"
                  class="form-control"
                  v-model="payPerPiece"
                  id="payPerPiece"
                  placeholder="Pay Per Piece"
                />

                <!-- For deflashing only -->
                <label for="chargePerPieceDelay" class="mt-2"
                  >Charge Per Piece Delay</label
                >
                <input
                  type="number"
                  class="form-control"
                  v-model="chargePerPieceDelay"
                  id="chargePerPieceDelay"
                  placeholder="Charge Per Piece Delay"
                />

                <label for="payPerPieceDelay" class="mt-2"
                  >Pay Per Piece Delay</label
                >
                <input
                  type="number"
                  class="form-control"
                  v-model="payPerPieceDelay"
                  id="payPerPieceDelay"
                  placeholder="Pay Per Piece Delay"
                />

                <label for="chargePerPieceExtraDelay" class="mt-2"
                  >Charge Per Piece Extra Delay</label
                >
                <input
                  type="number"
                  class="form-control"
                  v-model="chargePerPieceExtraDelay"
                  id="chargePerPieceExtraDelay"
                  placeholder="Charge Per Piece Extra Delay"
                />

                <label for="payPerPieceExtraDelay" class="mt-2"
                  >Pay Per Piece Extra Delay</label
                >
                <input
                  type="number"
                  class="form-control"
                  v-model="payPerPieceExtraDelay"
                  id="payPerPieceExtraDelay"
                  placeholder="Pay Per Piece Extra Delay"
                />

                <label for="employeePiecesPerHour" class="mt-2"
                  >Employee Pieces Per Hour</label
                >
                <input
                  type="number"
                  class="form-control"
                  v-model="employeePiecesPerHour"
                  id="employeePiecesPerHour"
                  placeholder="Employee Pieces Per Hour"
                />

                <label for="clientPiecesPerHour" class="mt-2"
                  >Client Pieces Per Hour</label
                >
                <input
                  type="number"
                  class="form-control"
                  v-model="clientPiecesPerHour"
                  id="clientPiecesPerHour"
                  placeholder="Client Pieces Per Hour"
                />
              </div>
            </form>
          </div>
          <div class="modal-footer d-flex justify-content-center">
            <button
              type="button"
              data-bs-dismiss="modal"
              class="btn btn-primary"
              @click="addQualifier()"
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
                  <h1 class="modal-title fs-5">Edit</h1>
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
                      <label for="responsiblePerson">Responsible Person</label>
                      <input
                        type="text"
                        class="form-control"
                        id="responsiblePerson"
                        v-model="props.row.responsiblePerson"
                        placeholder="Enter Responsible Person"
                      />

                      <label for="dateOfIssue" class="mt-2"
                        >Date of Issue</label
                      >
                      <input
                        type="text"
                        class="form-control"
                        v-model="props.row.dateOfIssue"
                        id="dateOfIssue"
                        placeholder="Date of Issue"
                      />

                      <label for="partNumber" class="mt-2">Part Number</label>
                      <input
                        type="text"
                        class="form-control"
                        v-model="props.row.partNumber"
                        id="partNumber"
                        placeholder="Part Number"
                      />

                      <label for="type" class="mt-2">Type</label>
                      <input
                        type="text"
                        class="form-control"
                        v-model="props.row.type"
                        id="type"
                        placeholder="Type"
                      />

                      <label for="tssCode" class="mt-2">TSS Code</label>
                      <input
                        type="text"
                        class="form-control"
                        v-model="props.row.tssCode"
                        id="tssCode"
                        placeholder="TSS Code"
                      />

                      <label for="chargePerPiece" class="mt-2"
                        >chargePerPiece</label
                      >
                      <input
                        type="text"
                        class="form-control"
                        v-model="props.row.chargePerPiece"
                        id="chargePerPiece"
                        placeholder="Charge Per Piece"
                      />

                      <label for="payPerPiece" class="mt-2"
                        >Pay Per Piece</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.payPerPiece"
                        id="payPerPiece"
                        placeholder="Pay Per Piece"
                      />

                      <!-- For deflashing only -->
                      <label for="chargePerPieceDelay" class="mt-2"
                        >Charge Per Piece Delay</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.chargePerPieceDelay"
                        id="chargePerPieceDelay"
                        placeholder="Charge Per Piece Delay"
                      />

                      <label for="payPerPieceDelay" class="mt-2"
                        >Pay Per Piece Delay</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.payPerPieceDelay"
                        id="payPerPieceDelay"
                        placeholder="Pay Per Piece Delay"
                      />

                      <label for="chargePerPieceExtraDelay" class="mt-2"
                        >Charge Per Piece Extra Delay</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.chargePerPieceExtraDelay"
                        id="chargePerPieceExtraDelay"
                        placeholder="Charge Per Piece Extra Delay"
                      />

                      <label for="payPerPieceExtraDelay" class="mt-2"
                        >Pay Per Piece Extra Delay</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.payPerPieceExtraDelay"
                        id="payPerPieceExtraDelay"
                        placeholder="Pay Per Piece Extra Delay"
                      />

                      <label for="employeePiecesPerHour" class="mt-2"
                        >Employee Pieces Per Hour</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.employeePiecesPerHour"
                        id="employeePiecesPerHour"
                        placeholder="Employee Pieces Per Hour"
                      />

                      <label for="clientPiecesPerHour" class="mt-2"
                        >Client Pieces Per Hour</label
                      >
                      <input
                        type="number"
                        class="form-control"
                        v-model="props.row.clientPiecesPerHour"
                        id="clientPiecesPerHour"
                        placeholder="Client Pieces Per Hour"
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
                      editQualifier(
                        props.row.id,
                        props.row.responsiblePerson,
                        props.row.dateOfIssue,
                        props.row.partNumber,
                        props.row.type,
                        props.row.tssCode,
                        props.row.chargePerPiece,
                        props.row.payPerPiece,
                        props.row.chargePerPieceDelay,
                        props.row.payPerPieceDelay,
                        props.row.chargePerPieceExtraDelay,
                        props.row.payPerPieceExtraDelay,
                        props.row.employeePiecesPerHour,
                        props.row.clientPiecesPerHour
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
          label: "Responsible Person",
          field: "responsiblePerson",
          type: "string",
        },
        {
          label: "Date of Issue",
          field: "dateOfIssue",
          type: "string",
        },
        {
          label: "Part Number",
          field: "partNumber",
          type: "strig",
        },
        {
          label: "Type",
          field: "type",
          type: "string",
        },
        {
          label: "TSS Code",
          field: "tssCode",
          type: "string",
        },
        {
          label: "ChargePerPiece",
          field: "chargePerPiece",
          type: "number",
        },
        {
          label: "PayPerPiece",
          field: "payPerPiece",
          type: "number",
        },
        //for deflashing only
        {
          label: "ChargePerPieceDelay",
          field: "chargePerPieceDelay",
          type: "number",
        },
        {
          label: "PayPerPieceDelay",
          field: "payPerPieceDelay",
          type: "number",
        },
        {
          label: "ChargePerPieceExtraDelay",
          field: "chargePerPieceExtraDelay",
          type: "number",
        },
        {
          label: "PayPerPieceExtraDelay",
          field: "payPerPieceExtraDelay",
          type: "number",
        },
        {
          label: "EmployeePiecesPerHour",
          field: "employeePiecesPerHour",
          type: "number",
        },
        {
          label: "ClientPiecesPerHour",
          field: "clientPiecesPerHour",
          type: "number",
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
    fetch("https://localhost:7164/Qualifiers")
      .then((res) => {
        if (res.ok) {
          return res.json();
        }
      })
      .then((data) => (this.rows = data))
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
  methods: {
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
    refreshData() {
      fetch("https://localhost:7164/Qualifiers")
        .then((res) => {
          if (res.ok) {
            return res.json();
          } else $toast.error("Error fetching Data");
        })
        .then((data) => (this.rows = data))
        .catch((err) => {
          $toast.error("Error fetching data") + err;
        });

      fetch("https://localhost:7164/Employee")
        .then((res) => {
          if (res.ok) {
            return res.json();
          }
        })
        .then((data) => (this.users = data))
        .catch((err) => {
          $toast.error("Error fetching Employees") + err;
        });
    },
    addQualifier() {
      apiInstance
        .post(`https://localhost:7164/Qualifiers`, {
          responsiblePerson: this.name,
          dateOfIssue: this.dateOfIssue,
          partNumber: this.partNumber,
          type: this.type,
          tssCode: this.tssCode,
          chargePerPiece: this.chargePerPiece,
          payPerPiece: this.payPerPiece,
          chargePerPieceDelay: this.chargePerPieceDelay,
          payPerPieceDelay: this.payPerPieceDelay,
          chargePerPieceExtraDelay: this.chargePerPieceExtraDelay,
          payPerPieceExtraDelay: this.payPerPieceExtraDelay,
          employeePiecesPerHour: this.employeePiecesPerHour,
          clientPiecesPerHour: this.clientPiecesPerHour,
        })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();

            $toast.success(`Created`);
          }
        })
        .catch((err) => $toast.error("Error creating") + err);
    },
    editQualifier(
      id,
      responsiblePerson,
      dateOfIssue,
      partNumber,
      type,
      tssCode,
      chargePerPiece,
      payPerPiece,
      chargePerPieceDelay,
      payPerPieceDelay,
      chargePerPieceExtraDelay,
      payPerPieceExtraDelay,
      employeePiecesPerHour,
      clientPiecesPerHour
    ) {
      const requestBody = {
        responsiblePerson: responsiblePerson,
        dateOfIssue: dateOfIssue,
        partNumber: partNumber,
        type: type,
        tssCode: tssCode,
        chargePerPiece: chargePerPiece,
        payPerPiece: payPerPiece,
        chargePerPieceDelay: chargePerPieceDelay,
        payPerPieceDelay: payPerPieceDelay,
        chargePerPieceExtraDelay: chargePerPieceExtraDelay,
        payPerPieceExtraDelay: payPerPieceExtraDelay,
        employeePiecesPerHour: employeePiecesPerHour,
        clientPiecesPerHour: clientPiecesPerHour,
      };

      apiInstance
        .put(`https://localhost:7164/Qualifiers/${id}`, requestBody)
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(`Edited`);
          }
        })
        .catch((err) => $toast.error("Error editing") + err);
    },
    remove(id) {
      apiInstance;
      fetch(`https://localhost:7164/Qualifiers/${id}`, { method: "DELETE" })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(`Qualifier ${id} deleted`);
          }
        })
        .catch((err) => $toast.error("Error deleting Employee ") + err);
    },
  },
};
</script>

<style>
</style>