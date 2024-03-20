<template>
  <div class="update-author-section">
      <label key="name">Name</label>
      <input type="text" v-model="name" id="name" placeholder="Name" @change="checkIsActive">
      <label key="biography">Biography</label>
      <textarea v-model="biography" id="biography" @change="checkIsActive"></textarea>

      <input type="checkbox" v-model="isKnownBirth" id="isKnownBirth" @change="checkIsActive">
      <label for="isKnownBirth">Birth date</label>
      <input type="date" v-model="birthYear" id="birthYear" :max="deathYear" :disabled="!isKnownBirth" @change="checkIsActive">

      <input class="checkbox-style" type="checkbox" v-model="isKnownDeath" id="isKnownDeath" @change="checkIsActive">
      <label for="isKnownDeath">Death date</label>
      <input type="date" v-model="deathYear" id="deathYear" :disabled="!isKnownDeath" @change="checkIsActive">

      <button @click="createAuthor" :class="{ 'button-update': isActive, 'button-update-disabled': !isActive }" :disabled="!isActive">Create</button>
  </div>
</template>

<script>
import * as listURL from "@/js/listUrl";


export default {
  data(){
      return{
          name: "",
          biography: "",
          birthYear: this.formatDate(Date.now()),
          deathYear: this.formatDate(Date.now()),
          isKnownBirth: false,
          isKnownDeath: false,
          isActive: false
      }
  },

  methods: {
      formatDate(date) {
          const d = new Date(date);
          const day = d.getDate().toString().padStart(2, '0');
          const month = (d.getMonth() + 1).toString().padStart(2, '0');
          const year = d.getFullYear();
          return `${year}-${month}-${day}`;
      },


      async checkIsActive(){
          if(this.name && this.biography && (!this.isKnownBirth || this.birthYear != this.formatDate(Date.now()))){
              this.isActive = true;
          }else{
              this.isActive = false;
          }
      },


      async createAuthor(){
          const dateBirth = this.isKnownBirth ? this.birthYear : null;
          const dateDeath = this.isKnownDeath ? this.deathYear : null;

          const data = {
              name: this.name,
              biography: this.biography,
              birthYear: dateBirth,
              deathYear: dateDeath
          }
          await listURL.requestPostCreateAuthor(data);

          this.name = "";
          this.biography = "";
          this.birthYear = this.formatDate(Date.now());
          this.deathYear = this.formatDate(Date.now());
          this.isActive = false;
      }
  },

  async mounted(){

  }
}
</script>

<style>

.update-author-section {
  padding-top: 120px;
  justify-content: center;
  display: grid;
}


.update-author-section label {
  font-size: x-large;
}

.update-author-section input[type="text"],
.update-author-section input[type="date"] {
  font-size: x-large;
  width: 650px;
  height: 50px;
  display: block;
  margin-bottom: 20px;
  border-radius: 15px;
}


.update-author-section input[type="checkbox"]{
  transform: scale(2);
}

.update-author-section textarea {
  font-size: x-large;
  width: 650px;
  height: 250px;
  display: block;
  margin-bottom: 20px;
  border-radius: 15px;
}

.button-update {
  color: white;
  position: center;
  width: 100px;
  height: 50px;
  text-align: center;
  background-color: rgb(0, 0, 0);
  border-radius: 15px;
  cursor: pointer;
  font-weight: blod;
  font-size: 1.2em;
  border: none;
}

.button-update:hover {
  background-color: rgb(48, 45, 45);
}


.button-update-disabled {
  color: white;
  position: center;
  width: 100px;
  height: 50px;
  text-align: center;
  background-color: rgb(119, 116, 116);
  border-radius: 15px;
  cursor: pointer;
  font-weight: blod;
  font-size: 1.2em;
  border: none;
}

</style>