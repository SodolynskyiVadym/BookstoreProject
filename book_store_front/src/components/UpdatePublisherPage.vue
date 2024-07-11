<template>
  <div class="main-section" v-if="loaded">
    <label key="name">Name</label><br>
    <input type="text" v-model="publisher.name" id="name" placeholder="Name" @change="checkIsActive"><br>

    <label key="photo">Photo</label><br>
    <input type="text" v-model="publisher.imageUrl" @change="checkIsActive"><br>
    <button @click="updatePublisher" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }"
            :disabled="!isActive">Update
    </button>
  </div>
</template>

<script>
import * as publisherAPI from "@/js/API/publisherAPI";

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
      this.isActive = this.publisher.name && this.publisher.imageUrl;
    },


    async updatePublisher() {
      const data = {
        id: this.$route.params.id,
        name: this.publisher.name,
      }
      await publisherAPI.patchUpdatePublisher(data);
    }
  },

  async mounted() {
    this.publisher = await publisherAPI.getPublisher(this.$route.params.id);
    if (this.publisher) this.loaded = true;
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>