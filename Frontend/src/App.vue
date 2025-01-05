<template>
  
  <nav class="navbar bg-white shadow-sm fixed-top" >
    <div class="container">
      <div class="border rounded p-2 me-1 mx-lg-3">
        <router-link to="/employee" class="navbar-link">Employee</router-link>
      </div>
      <div>
        <router-link to="/" class="navbar-brand">InspectraTSS</router-link>
      </div>
      <div class="navbar-right border rounded p-2 mx-lg-3">
        <router-link to="/upload" class="navbar-link">Upload</router-link>
      </div>
    </div>
  </nav>
  <div style="padding-top:4.5em;">
    <router-view />
  
  </div>
</template>

<script>
export default {
  name: 'App',
  data() {
    return {
    }
  },
  methods: {
   
  },
}
</script>


<style scoped>
@import "./assets/css/style.css";

.navbar-brand{
  font-size: 1.8em;
  color: #008cba;
}
.navbar-brand:hover{
  color: #00526e;
}
.navbar-link{
  color:#008cba;
  text-decoration: none;
  font-size: 1.2em;
}
.navbar-link:hover{
  color: #00526e;
}


</style>
