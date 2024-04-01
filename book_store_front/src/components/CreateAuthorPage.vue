<template>
  <div class="main-section">
    <label key="name">Name</label><br>
    <input type="text" v-model="name" id="name" placeholder="Name" @change="checkIsActive"><br>
    <label key="biography">Biography</label><br>
    <textarea v-model="biography" id="biography" @change="checkIsActive"></textarea><br>

    <input type="checkbox" v-model="isKnownBirth" id="isKnownBirth" @change="checkIsActive"><br>
    <label for="isKnownBirth">Birth date</label><br>
    <input type="date" v-model="birthYear" id="birthYear" :max="deathYear" :disabled="!isKnownBirth" @change="checkIsActive"><br>

    <input class="checkbox-style" type="checkbox" v-model="isKnownDeath" id="isKnownDeath" @change="checkIsActive"><br>
    <label for="isKnownDeath">Death date</label><br>
    <input type="date" v-model="deathYear" id="deathYear" :disabled="!isKnownDeath" @change="checkIsActive"><br>

    <label>Photo file</label><br>
    <input type="file" ref="fileInput" accept=".jpg, .jpeg" @change="checkIsActive"><br>

    <button @click="createAuthor" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }"
            :disabled="!isActive">Create
    </button><br>
  </div>
</template>

<script>
import * as listURL from "@/js/listUrl";
import * as dateHelper from "@/js/dateHelper";

export default {
  data() {
    return {
      name: "",
      biography: "",
      birthYear: dateHelper.formatDate(Date.now()),
      deathYear: dateHelper.formatDate(Date.now()),
      isKnownBirth: false,
      isKnownDeath: false,
      isActive: false
    }
  },

  methods: {
    async checkIsActive() {
      this.isActive = this.name && this.biography && (!this.isKnownBirth || this.birthYear !== dateHelper.formatDate(Date.now())) && this.$refs.fileInput.value;
    },


    async createAuthor() {
      const token = localStorage.getItem("token");
      if (token){
        const dateBirth = this.isKnownBirth ? this.birthYear : null;
        const dateDeath = this.isKnownDeath ? this.deathYear : null;

        const data = {
          name: this.name,
          biography: this.biography,
          birthYear: dateBirth,
          deathYear: dateDeath
        }
        await listURL.requestPostCreateAuthor(data, token);

        const file = this.$refs.fileInput.files[0];
        const formData = new FormData();
        formData.append('file', file);
        formData.append('biography', this.biography);
        formData.append('name', this.name);

        await listURL.requestCreateAuthorImage(formData, token);

        this.$refs.fileInput.value = ''
        this.name = "";
        this.biography = "";
        this.birthYear = dateHelper.formatDate(Date.now());
        this.deathYear = dateHelper.formatDate(Date.now());
        this.isActive = false;
      }

    }
  },

  async mounted() {

  }
}
</script>

<style>

@import "@/assets/css/styles.css";


</style>