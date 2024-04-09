<template>
  <div class="main-section" v-if="loaded">
    <label key="name">Name</label><br>
    <input type="text" v-model="publisher.name" id="name" placeholder="Name" @change="checkIsActive"><br>
    <button @click="updatePublisher" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }"
            :disabled="!isActive">Update
    </button>
  </div>
</template>

<script>
import * as listURL from "@/js/listUrl";


export default {
  data() {
    return {
      publisher: null,
      isActive: true,
      loaded: false
    }
  },

  methods: {
    async checkIsActive() {
      if (this.publisher.name) {
        this.isActive = true;
      } else {
        this.isActive = false;
      }
    },


    async updatePublisher() {
      const data = {
        id: this.$route.params.id,
        name: this.publisher.name,
      }
      await listURL.patchUpdatePublisher(data);
    }
  },

  async mounted() {
    this.publisher = await listURL.getPublisher(this.$route.params.id);

    console.log(this.publisher)

    if (this.publisher) this.loaded = true;
  }
}
</script>

<style>

@import "@/assets/css/styles.css";

</style>