
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
          style="width: 2.4em; height: 2.4em"
          data-bs-toggle="modal"
          :data-bs-target="'#edit' + props.row.id"
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
          :id="'edit' + props.row.id"
          tabindex="-1"
          aria-hidden="true"
        >
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <h1 class="modal-title fs-5">Edit {{ props.row.id }}</h1>
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
                    <div class="row">
                      <span class="col-4">
                        <label>Mo Number</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.moNumber"
                          placeholder="Mo Number"
                        />
                      </span>
                      <span class="col-4">
                        <label>Chit Number</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.chitNumber"
                          placeholder="Chit Number"
                        />
                      </span>
                      <span class="col-4">
                        <label>Part Number</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.partNumber"
                          placeholder="Part Number"
                        />
                      </span>
                    </div>
                    <div class="row my-3">
                      <span class="col-4">
                        <label>Supplier</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.supplier"
                          placeholder="Supplier"
                        />
                      </span>
                      <span class="col-4">
                        <label>Inspection Type</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.inspectionType"
                          placeholder="Inspection Type"
                        />
                      </span>
                      <span class="col-4">
                        <label>Quality</label>
                        <input
                          type="number"
                          class="form-control"
                          v-model="props.row.quality"
                          placeholder="Quality"
                        />
                      </span>
                    </div>
                    <div class="row">
                      <span class="col-4">
                        <label>Quantity Issued</label>
                        <input
                          type="number"
                          class="form-control"
                          v-model="props.row.quantityIssued"
                          placeholder="Quantity Issued"
                        />
                      </span>
                      <span class="col-4">
                        <label>Date Issued</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.dateIssued"
                          placeholder="Date Issued"
                        />
                      </span>
                      <span class="col-4">
                        <label>Requested Date</label>
                        <input
                          type="text"
                          class="form-control"
                          v-model="props.row.requestedDate"
                          placeholder="Requested Date"
                        />
                      </span>
                    </div>
                  </div>
                </form>
              </div>
              <div class="modal-footer d-flex justify-content-center">
                <button
                  type="button"
                  data-bs-dismiss="modal"
                  class="btn btn-primary"
                  @click="
                    EditBag(
                      props.row.id,
                      props.row.bagNumber,
                      props.row.moNumber,
                      props.row.chitNumber,
                      props.row.partNumber,
                      props.row.supplier,
                      props.row.inspectionType,
                      props.row.quality,
                      props.row.quantityIssued,
                      props.row.dateIssued,
                      props.row.requestedDate
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
      <span v-if="props.column.field == 'assignedTo'">
        <select
          class="form-select"
          v-model="props.row.assignedTo"
          @change="assignBag(props.row.id, props.row.assignedTo)"
        >
          <option default :value="null"></option>
          <option v-for="user in users" :key="user.id" :value="user.id">
            {{ user.punchCode }}({{ user.name }} {{ user.surname }})
          </option>
        </select>
      </span>

      <span
        v-if="
          props.column.field == 'return' &&
          props.row.assigned == true &&
          props.row.quantityIssued !== 0
        "
      >
        <button
          type="button"
          class="btn btn-success"
          data-bs-toggle="modal"
          :data-bs-target="'#' + props.row.id"
        >
          Received
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
                <h1 class="modal-title fs-5">Check Bag</h1>
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
                    <div class="row">
                      <div class="col-6">
                        <label
                          for="quantityIssued"
                          style="font-weight: bold; font-size: 18px"
                          >Quantity issued:</label
                        >
                        <span style="font-weight: bold; font-size: 18px">{{
                          props.row.quantityIssued
                        }}</span>
                      </div>
                      <br /><br />
                    </div>
                    <div class="row">
                      <div class="col-6">
                        <label for="quantity_passed">Quantity Passed</label>
                        <input
                          type="number"
                          class="form-control"
                          id="quantity_passed"
                          v-model="props.row.quantity_passed"
                          placeholder="Quantity Passed"
                        />
                      </div>
                      <div class="col-6">
                        <label for="quantity_failed">Quantity Failed</label>
                        <input
                          type="number"
                          class="form-control"
                          v-model="props.row.quantity_failed"
                          id="quantity_failed"
                          placeholder="Quantity Failed"
                        />
                      </div>
                    </div>
                    <v-textarea
                      class="mt-5"
                      label="Notes"
                      variant="solo"
                      v-model="props.row.notes"
                    ></v-textarea>
                  </div>
                </form>
              </div>
              <div class="modal-footer d-flex justify-content-center">
                <button
                  type="button"
                  data-bs-dismiss="modal"
                  class="btn btn-primary"
                  @click="
                    ReturnGood(
                      props.row.id,
                      props.row.quantity_passed,
                      props.row.quantity_failed,
                      props.row.notes
                    )
                  "
                >
                  Confirm
                </button>
              </div>
            </div>
          </div>
        </div>
      </span>
    </template>
  </vue-good-table>
</template>

<script>
import { token } from "./token.js";
export const apiInstance = axios.create({
  baseURL: "https://localhost:7164",
  responseType: "json",
  headers: {
    //'Content-Type': 'application/json',
    // you dont need to pre-define all headers
  },
});
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
      token: token,
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
          label: "",
          field: "return",
          sortable: false,
        },
        {
          label: "",
          field: "edit",
          sortable: false,
        },
      ],
      rows: [],
      codes: [],
      users: [],
    };
  },
  methods: {
    ReturnGood(id, quantityPassed, quantityFailed, notes) {
      apiInstance
        .put(`https://localhost:7164/BagInventory/UpdateBagQuantities`, {
          id: id,
          quantityPassed: quantityPassed,
          quantityFailed: quantityFailed,
          notes: notes,
        })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(
              `Bag returned with ${quantityPassed} passed and ${quantityFailed} failed`
            );
          }
        })
        .catch(function (err) {
          console.log(err);
          $toast.error(`${err.response.data}`);
        });
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
    EditBag(
      id,
      bagNumber,
      moNumber,
      chitNumber,
      partNumber,
      supplier,
      inspectionType,
      quality,
      quantityIssued,
      dateIssued,
      requestedDate
    ) {
      apiInstance
        .put(`https://localhost:7164/BagInventory/${id}`, {
          bagNumber: bagNumber,
          moNumber: moNumber,
          chitNumber: chitNumber,
          partNumber: partNumber,
          supplier: supplier,
          inspectionType: inspectionType,
          quality: quality,
          quantityIssued: quantityIssued,
          dateIssued: dateIssued,
          requestedDate: requestedDate,
        })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(`Bag ${bagNumber} Edited`);
          }
        })
        .catch((err) => $toast.error("Error editing bag") + err);
    },
    remove(id) {
      apiInstance;
      fetch(`https://localhost:7164/BagInventory/Delete/${id}`, {
        method: "DELETE",
      }).then((result) => {
        if (result.ok) {
          $toast.success(`Bag Number with id (${id}) deleted`);
          this.refreshData();
        } else $toast.error(`Error deleting data ${id}`);
      });
    },
    refreshData() {
      fetch("https://localhost:7164/BagInventory/GetUploadedBags")
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

    assignBag(bagInventoryId, employeeId) {
      console.log(typeof bagInventoryId + " " + Number(employeeId));
      apiInstance
        .put(`https://localhost:7164/BagInventory/AssignEmployee`, {
          bagInventoryId: bagInventoryId,
          employeeId: Number(employeeId),
        })
        .then((result) => {
          if (result.status >= 200 && result.status <= 205) {
            this.refreshData();
            $toast.success(` ${bagInventoryId} assigned to employee`);
          }
        })
        .catch((err) => $toast.error("Error assigning bag") + err);
    },
  },

  mounted() {
    fetch("https://localhost:7164/BagInventory/GetUploadedBags")
      .then((res) => {
        if (res.ok) {
          return res.json();
        } else $toast.error("Error fetching Data");
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
};
</script>

<style scoped>
@import url("https://unpkg.com/css.gg@2.0.0/icons/css/trash.css");
@import url("https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.3/font/bootstrap-icons.css");
</style>