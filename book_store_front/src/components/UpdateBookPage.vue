<template>

  <div class="main-section" v-if="loaded">
    <label key="name">Name</label><br>
    <input id="name" type="text" v-model="book.name" placeholder="Name" @change="checkIsActiveButton"><br>

    <label key="language">Language</label><br>
    <input id="language" type="text" v-model="book.bookLanguage" placeholder="Language" @change="checkIsActiveButton"><br>

    <label key="price">Price</label><br>
    <input id="price" type="number" v-model="book.price" min="0" @change="checkIsActiveButton"><br>

    <label key="discount">Discount</label><br>
    <input id="discount" type="number" v-model="book.discount" min="0" max="100" @change="checkIsActiveButton"><br>

    <label key="authorName">Author</label><br>
    <select id="authorName" v-model="authorName" @change="findIdByNameAuthor">
      <option v-if="authors.length === 0" disabled>No authors available</option>
      <option v-for="author in authors" :key="author.id" v-text="author.name"></option>
    </select><br>

    <label key="publisherName">Publisher</label><br>
    <select id="publisherName" v-model="publisherName" @change="findIdByNamePublisher">
      <option v-if="publishers.length === 0" disabled>No publishers available</option>
      <option v-for="publisher in publishers" :key="publisher.id" v-text="publisher.name"></option>
    </select><br>

    <label key="quantity">Available quantity</label><br>
    <input type="number" v-model="book.availableQuantity" placeholder="Available quantity"><br>

    <label key="numberPages">Pages number</label><br>
    <input id="numberPages" type="number" v-model="book.numberPages" @change="checkIsActiveButton"><br>

    <label key="yearPublication">Year publication</label><br>
    <input id="yearPublication" type="Date" v-model="book.yearPublication" :max="maxDate" @change="checkIsActiveButton"><br>

    <label key="description">Description</label><br>
    <textarea v-model="book.description" id="description" placeholder="Description" @change="checkIsActiveButton"></textarea><br>

    <button @click="updateBook" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Update</button>
  </div>

</template>

<script>
import * as listURL from "@/js/listUrl";
import * as dateHelper from "@/js/dateHelper";

export default {
  data() {
    return {
      publisherName: "",
      authorName: "",
      book: null,
      publishers: [],
      authors: [],
      maxDate: dateHelper.formatDate(Date.now()),
      isActive: true,
      loaded: false
    }
  },

  methods: {
    async findIdByNameAuthor() {
      this.book.authorId = this.authors.find(author => author.name === this.authorName).id;
      console.log(this.book.authorId);
    },


    async findIdByNamePublisher() {
      this.book.publisherId = this.publishers.find(publsher => publsher.name === this.publisherName).id;
      console.log(this.book.publisherId);
    },


    async checkIsActiveButton() {
      this.isActive = this.book.name && this.book.description && this.book.numberPages >= 0 && this.book.bookLanguage && this.book.publisherId > 0
          && this.book.authorName && this.book.authorId > 0 && this.publisherName && this.book.price >= 0 && this.book.discount >= 0
          && this.book.discount <= 100 && this.book.availableQuantity >= 0;
    },


    async updateBook() {

      const data = {
        id: this.book.id,
        name: this.book.name,
        description: this.book.description,
        numberPages: this.book.numberPages,
        bookLanguage: this.book.bookLanguage,
        yearPublication: this.book.yearPublication,
        publisherId: this.book.publisherId,
        authorId: this.book.authorId,
        availableQuantity: this.book.availableQuantity,
        price: this.book.price,
        discount: this.book.discount,
      };

      await listURL.patchUpdateBook(data);
    }
  },

  async mounted() {
    this.publishers = await listURL.getAllPublishers();
    this.authors = await listURL.getAllAuthors();
    this.book = await listURL.getBook(this.$route.params.id);

    this.book.yearPublication = dateHelper.formatDate(this.book.yearPublication);
    this.authorName = this.authors.find(author => author.id === this.book.authorId).name;
    this.publisherName = this.publishers.find(publisher => publisher.id === this.book.publisherId).name;
    this.loaded = true;
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>