<template>
  <div v-if="loaded">
    <div class="author-nameBook">
      <div>{{ book.authorName }}</div>
      <div>Book "{{book.name}}"</div>
    </div>
    <div class="book-info">
      <img class="img-book" :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)">
    </div>
  </div>
</template>


<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
    return {
      book: null,
      bookPhotoUrl: "",
      loaded: false
    };
  },

  async mounted() {
    this.book = await listURL.requestGetBook(this.$route.params.id);
    // this.bookPhotoUrl = `@/assets/bookPhoto/${this.book.name.toLowerCase().replace(/\s+/g, '')}${this.book.id}.jpg`;
    console.log(this.bookPhotoUrl)
    this.loaded = true;
  }
}
</script>

<style>

</style>