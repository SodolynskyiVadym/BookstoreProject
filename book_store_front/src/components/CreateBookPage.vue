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

    <label key="photo">Photo</label>
    <input type="text" id="photo" v-model="imageUrl" @change="checkIsActiveButton"><br>
    
    <label key="genres">Genres</label><br>
    <div v-for="(genre, index) in genres" :key="index">
      <input type="search" list="genres" v-model="genres[index]"><br>
      <datalist id="genres">
        <option v-for="choosedGenre in genresList" :key="choosedGenre" :value="choosedGenre">{{ choosedGenre }}</option>
      </datalist>
      <button @click="removeGenre(index)" style="background-color: red; height: auto;" class="main-button">Remove genre</button>
    </div><br>

    <button @click="addGenre" style="background-color: green;" class="main-button">Add genre</button><br>
    <button @click="createBook" style="margin-top: 30px;" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Create</button>
  </div>

</template>

<script>
import * as bookAPI from "@/js/API/bookAPI";
import * as authorAPI from "@/js/API/authorAPI";
import * as publisherAPI from "@/js/API/publisherAPI";
import * as dateHelper from "@/js/dateHelper";
import * as genresChoices from "@/js/genres";

export default {
  data() {
    return {
      name: "",
      description: "",
      numberPages: 0,
      bookLanguage: "",
      yearPublication: dateHelper.formatDate(Date.now()),
      maxDate: dateHelper.formatDate(Date.now()),
      publisherId: 0,
      publisherName: "",
      authorId: 0,
      authorName: "",
      imageUrl: "",
      availableQuantity: 0,
      price: 0,
      discount: 0,
      publishers: [],
      authors: [],
      genres: [],
      genresList: genresChoices.genres,

      isActive: false
    }
  },

  methods: {
    async findIdByNameAuthor() {
      this.authorId = this.authors.find(author => author.name === this.authorName).id;
      console.log(this.authorId);
    },

    async addGenre() {
      this.genres.push("");
    },

    async removeGenre(index) {
      this.genres.splice(index, 1);
    },

    async findIdByNamePublisher() {
      this.publisherId = this.publishers.find(publsher => publsher.name === this.publisherName).id;
      console.log(this.publisherId);
    },


    async checkIsActiveButton() {
      if (this.name && this.description && this.numberPages >= 0 && this.bookLanguage && this.publisherId > 0 && this.authorName && this.authorId > 0
          && this.publisherName && this.price >= 0 && this.discount >= 0 && this.discount <= 100 && this.imageUrl) {
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
        imageUrl: this.imageUrl,
        genres: this.genres
      };

      const token = localStorage.getItem("token");
      await bookAPI.postCreateBook(data, token);


      this.imageUrl = ''
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
    console.log(await authorAPI.getAllAuthors())
    console.log(this.publishers);
    console.log(this.authors);
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>