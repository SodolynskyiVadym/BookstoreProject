<template>
  <div class="main-section" v-if="loaded">
    <label key="name">Name</label>
    <input type="text" v-model="author.name" id="name" placeholder="Name" @change="checkIsActive"><br>
    <label key="biography">Biography</label><br>
    <textarea v-model="author.biography" id="biography" @change="checkIsActive"></textarea><br>

    <label for="isKnownBirth">Birth date</label><br>
    <input type="checkbox" v-model="isKnownBirth" id="isKnownBirth" @change="checkIsActive"><br>
    <input type="date" v-model="author.birthYear" id="birthYear" :max="author.deathYear" :disabled="!isKnownBirth" @change="checkIsActive"><br>

    <label for="isKnownDeath">Death date</label><br>
    <input class="checkbox-style" type="checkbox" v-model="isKnownDeath" id="isKnownDeath" @change="checkIsActive"><br>
    <input type="date" v-model="author.deathYear" id="deathYear" :disabled="!isKnownDeath" @change="checkIsActive"><br>

    <button @click="updateAuthor" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Update</button>
  </div>
</template>

<script>
import * as listURL from "@/js/listUrl";
import * as authorAPI from "@/js/API/authorAPI";
import * as dateHelper from "@/js/dateHelper";

export default {
  data(){
      return{
          author: null,
          isKnownBirth: false,
          isKnownDeath: false,
          isActive: true,
          loaded: false
      }
  },

  methods: {
      async checkIsActive(){
        console.log("code work")
        console.log(this.author.name && this.author.biography && (!this.isKnownBirth || this.author.birthYear !== dateHelper.formatDate(Date.now())));
          if(this.author.name && this.author.biography && (!this.isKnownBirth || this.author.birthYear !== dateHelper.formatDate(Date.now()))){
              this.isActive = true;
          }else{
              this.isActive = false;
          }
      },


      async updateAuthor(){
          const dateBirth = this.isKnownBirth ? this.author.birthYear : null;
          const dateDeath = this.isKnownDeath ? this.author.deathYear : null;

          const data = {
            id: this.$route.params.id,
            name: this.author.name,
            biography: this.author.biography,
            birthYear: dateBirth,
            deathYear: dateDeath
          }
          await authorAPI.patchUpdateAuthor(data);
      }
  },

  async mounted(){
    this.author = await authorAPI.getAuthor(this.$route.params.id);
    if(this.author.deathYear){
      this.author.deathYear = dateHelper.formatDate(this.author.deathYear);
      this.isKnownDeath = true;
    }else this.author.deathYear = dateHelper.formatDate(Date.now());


    if(this.author.birthYear){
      this.author.birthYear = dateHelper.formatDate(this.author.birthYear);
      this.isKnownBirth = true;
    }else this.author.birthYear = dateHelper.formatDate(Date.now());

    if(this.author) this.loaded = true;
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>