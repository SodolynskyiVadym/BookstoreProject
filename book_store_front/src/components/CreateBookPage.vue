<template>
  <div class="main-section">
    <label key="name">Name</label><br>
    <input id="name" type="text" v-model="name" placeholder="Name" @change="checkIsActiveButton"><br>
    <label key="language">Language</label><br>
    <input id="language" type="text" v-model="bookLanguage" placeholder="Language" @change="checkIsActiveButton"><br>
    <label key="price">Price</label><br>
    <input id="price" type="number" v-model="price" min="0" @change="checkIsActiveButton"><br>
    <label key="discount">Discount</label><br>
    <input id="discount" type="number" v-model="discount" min="0" max="100" @change="checkIsActiveButton"><br>
    <label>Available quantity</label><br>
    <input type="text" v-model="availableQuantity" @change="checkIsActiveButton" min="0"><br>
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
    <label key="numberPages">Pages number</label><br>
    <input id="numberPages" type="number" v-model="numberPages" @change="checkIsActiveButton"><br>
    <label key="yearPublication">Year publication</label><br>
    <input id="yearPublication" type="Date" v-model="yearPublication" :max="maxDate" @change="checkIsActiveButton"><br>
    <label key="description">Description</label><br>
    <textarea v-model="description" id="description" placeholder="Description" @change="checkIsActiveButton"></textarea><br>
    <input type="file" ref="fileInput" @change="checkIsActiveButton"><br>
    <button @click="createBook" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Create</button>
  </div>

</template>

<script>
import * as bookAPI from "@/js/API/bookAPI";
import * as authorAPI from "@/js/API/authorAPI";
import * as publisherAPI from "@/js/API/publisherAPI";
import * as dateHelper from "@/js/dateHelper";

export default {
  data() {
    return {
      name: "",
      description: "",
      numberPages: 0,
      bookLanguage: "",
      yearPublication: this.formatDate(Date.now()),
      maxDate: this.formatDate(Date.now()),
      publisherId: 0,
      publisherName: "",
      authorId: 0,
      authorName: "",
      availableQuantity: 0,
      price: 0,
      discount: 0,
      publishers: [],
      authors: [],
      isActive: false
    }
  },

  methods: {
    async findIdByNameAuthor() {
      this.authorId = this.authors.find(author => author.name === this.authorName).id;
      console.log(this.authorId);
    },


    async findIdByNamePublisher() {
      this.publisherId = this.publishers.find(publsher => publsher.name === this.publisherName).id;
      console.log(this.publisherId);
    },


    async checkIsActiveButton() {
      if (this.name && this.description && this.numberPages >= 0 && this.bookLanguage && this.publisherId > 0 && this.authorName && this.authorId > 0
          && this.publisherName && this.price >= 0 && this.discount >= 0 && this.discount <= 100 && this.$refs.fileInput.value) {
        this.isActive = true;
      } else {
        this.isActive = false;
      }
    },


    async createBook() {
      const data = {
        name: this.name,
        description: this.description,
        numberPages: this.numberPages,
        bookLanguage: this.bookLanguage,
        yearPublication: this.yearPublication,
        publisherId: this.publisherId,
        authorId: this.authorId,
        availableQuantity: this.availableQuantity,
        price: this.price,
        discount: this.discount,
        selectedFile: null
      };

      const token = localStorage.getItem("token");
      await bookAPI.postCreateBook(data, token);

      const file = this.$refs.fileInput.files[0];
      const formData = new FormData();
      formData.append('file', file);
      formData.append('authorId', this.authorId);
      formData.append('name', this.name);

      await bookAPI.postCreateBookImage(formData, token);

      this.$refs.fileInput.value = ''
      this.isActive = false;
      this.name = "";
      this.description = "";
      this.numberPages = 0;
      this.bookLanguage = "";
      this.yearPublication = dateHelper.formatDate(Date.now());
      this.publisherName = "";
      this.authorName = "";
      this.availableQuantity = 0;
      this.price = 0;
      this.discount = 0;
    }
  },

  async mounted() {
    this.publishers = await publisherAPI.getAllPublishers();
    this.authors = await authorAPI.getAllAuthors();
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>