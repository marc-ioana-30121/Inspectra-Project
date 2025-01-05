<template>
  <div class="d-flex justify-content-center mt-5">
    <div class="text-center upload">
      <h1 class="pb-5">Upload a file</h1>
      <label class="drop-container">
        <span class="drop-title">Drop files here</span>
        or
        <input
          type="file"
          id="xlsx"
          enctype="multipart/form-data"
          accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
          required
        />
      </label>

      <button
        type=""
        class="btn btn-primary btn-lg mt-5 ps-5 pe-5"
        @click="post"
      >
        Submit
      </button>

      <div>
        <p v-if="bagNumbers.length === 0">No duplicated bags</p>
        <p v-else>Duplicated bags founded in file:</p>
        <ul v-if="bagNumbers.length > 0">
          <li v-for="bagNumber in bagNumbers" :key="bagNumber">
            {{ bagNumber }}
          </li>
        </ul>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { useToast } from "vue-toast-notification";
import XLSX from "xlsx";
const $toast = useToast();
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
  name: "UploadGoods",
  data() {
    return {
      imported: [],
      bagNumbers: [],
    };
  },
  methods: {
    post() {
      const reader = new FileReader();
      reader.onload = (event) => {
        const fileData = event.target.result;

        const workbook = XLSX.read(fileData, { type: "binary" });
        const sheetName = workbook.SheetNames[0];
        const sheet = workbook.Sheets[sheetName];
        const jsonData = XLSX.utils.sheet_to_json(sheet);

        console.log(jsonData);
      };
      var formFile = document.getElementById("xlsx").files[0];
      const formData = new FormData();
      formData.append("formFile", formFile);
      const formDataObject = {};
      for (const [key, value] of formData.entries()) {
        formDataObject[key] = value;
      }

      console.log(formDataObject);

      apiInstance
        .post(`https://localhost:7164/BagInventory/UploadBag`, formData)
        .then((result) => {
          if (result.status >= 200) {
            $toast.success(`Uploaded succesfully!`);
            result.data.forEach((item) => {
              if (item.bagNumber) {
                this.bagNumbers.push(item.bagNumber); // Adăugăm bagNumber în array-ul nostru
              }
            });
          }
        })
        .catch(function (err) {
          console.log(err);
          $toast.error(`${err.response.data}`);
        });
    },
  },
};
</script>

<style scoped>
.upload {
  width: 70%;
}

.drop-container {
  position: relative;
  display: flex;
  gap: 10px;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 200px;
  padding: 20px;
  border-radius: 10px;
  border: 2px dashed #555;
  color: #444;
  cursor: pointer;
  transition: background 0.2s ease-in-out, border 0.2s ease-in-out;
}

.drop-container:hover {
  background: #eee;
  border-color: #111;
}

.drop-container:hover .drop-title {
  color: #222;
}

.drop-title {
  color: #444;
  font-size: 20px;
  font-weight: bold;
  text-align: center;
  transition: color 0.2s ease-in-out;
}

input[type="file"] {
  width: 350px;
  max-width: 100%;
  color: #444;
  padding: 5px;
  background: #fff;
  border-radius: 10px;
  border: 1px solid #555;
}

input[type="file"]::file-selector-button {
  margin-right: 20px;
  border: none;
  background: #0d6efd;
  padding: 10px 20px;
  border-radius: 10px;
  color: #fff;
  cursor: pointer;
  transition: background 0.2s ease-in-out;
}

input[type="file"]::file-selector-button:hover {
  background: #0b5ed7;
}
</style>