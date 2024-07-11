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

    <label>Photo</label><br>
    <input type="text" v-model="imageUrl" @change="checkIsActive"><br>

    <button @click="createAuthor" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }"
            :disabled="!isActive">Create
    </button><br>
  </div>
</template>

<script>
import * as authorAPI from "@/js/API/authorAPI";
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
      imageUrl: "",
      isActive: false
    }
  },

  methods: {
    async checkIsActive() {
      this.isActive = this.name && this.biography && (!this.isKnownBirth || this.birthYear !== dateHelper.formatDate(Date.now())) && this.imageUrl.length < 255;
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
          deathYear: dateDeath,
          imageUrl: this.imageUrl
        }
        await authorAPI.postCreateAuthor(data, token);

        this.name = "";
        this.biography = "";
        this.birthYear = dateHelper.formatDate(Date.now());
        this.deathYear = dateHelper.formatDate(Date.now());
        this.imageUrl = "";
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