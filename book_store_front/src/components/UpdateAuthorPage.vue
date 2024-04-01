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
      formatDate(date) {
          const d = new Date(date);
          const day = d.getDate().toString().padStart(2, '0');
          const month = (d.getMonth() + 1).toString().padStart(2, '0');
          const year = d.getFullYear();
          return `${year}-${month}-${day}`;
      },


      async checkIsActive(){
        console.log("code work")
        console.log(this.author.name && this.author.biography && (!this.isKnownBirth || this.author.birthYear != this.formatDate(Date.now())));
          if(this.author.name && this.author.biography && (!this.isKnownBirth || this.author.birthYear != this.formatDate(Date.now()))){
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
          await listURL.requestPatchUpdateAuthor(data);
      }
  },

  async mounted(){
    this.author = await listURL.requestGetAuthor(this.$route.params.id);
    if(this.author.deathYear){
      this.author.deathYear = this.formatDate(this.author.deathYear);
      this.isKnownDeath = true;
    }else this.author.deathYear = this.formatDate(Date.now());


    if(this.author.birthYear){
      this.author.birthYear = this.formatDate(this.author.birthYear);
      this.isKnownBirth = true;
    }else this.author.birthYear = this.formatDate(Date.now());

    if(this.author) this.loaded = true;
  }
}
</script>

<style>

@import "@/assets/css/styles.css";

</style>