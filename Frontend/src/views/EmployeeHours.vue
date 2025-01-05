<template>
  <div>
    <date-picker
      v-model="startDate"
      format="MM / dd / yyyy"

      placeholder="Start date"

      placeholder="Start Date"

      :class="custom - date - picker"
    >
      <template v-slot:icon-calendar></template
    ></date-picker>
    <date-picker
      v-model="finalDate"
      format="MM / dd / yyyy"

      placeholder="End date"

      placeholder="End Date"
      :class="custom - date - picker"
    >
      <template v-slot:icon-calendar> </template
    ></date-picker>

    <button @click="sendData">Generate</button

    <button @click="sendData">Generate Excel</button

    ><a ref="downloadLink" style="display: none"></a>
  </div>
</template>
  
  <script>
import DatePicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";
export const apiInstance = axios.create({
  baseURL: "https://localhost:7164",
  responseType: "json",
  headers: {},
});
import { token } from "./token.js";
apiInstance.defaults.headers.common["Authorization"] = `Bearer ${token}`;

import axios from "axios";
import { useToast } from "vue-toast-notification";
const $toast = useToast();
export default {
  components: {
    DatePicker,
  },
  data() {
    return {
      startDate: null,
      finalDate: null,
    };
  },
  methods: {
    sendData() {
      const formattedStartDate = this.formatDate(this.startDate);
      const formattedFinalDate = this.formatDate(this.finalDate);
      console.log(formattedFinalDate);
      if (this.startDate && this.finalDate) {
        const url = `https://localhost:7164/Qualifiers/EmployeeWageTable?startDate=${formattedStartDate}&finalDate=${formattedFinalDate}`;

        apiInstance
          .get(url)
          .then((result) => {
            if (result.status >= 200 && result.status <= 205) {
              $toast.success(`Download succesfully`);
              const downloadUrl = `https://localhost:7164/Qualifiers/EmployeeWageTable?startDate=${formattedStartDate}&finalDate=${formattedFinalDate}`;

              this.$refs.downloadLink.setAttribute("href", downloadUrl);

              this.$refs.downloadLink.click();
            }
          })
          .catch((err) => $toast.error("Error") + err);
      } else {
        console.error("No data selected.");
      }
    },
    formatDate(date) {
      if (!date) return "";
      const dd = String(date.getDate()).padStart(2, "0");
      const mm = String(date.getMonth() + 1).padStart(2, "0");
      const yyyy = date.getFullYear();
      return mm + "/" + dd + "/" + yyyy;
    },
  },
};
</script>
<style scoped>
.custom-date-picker {
  width: 50px;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  outline: none;
  cursor: pointer;
}

button {
  padding: 10px 20px;
  background-color: #008cba;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #007aa9;
}
</style>
  