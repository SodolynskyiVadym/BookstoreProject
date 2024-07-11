<template>

  <div class="main-section" v-if="loaded">
    <label key="name">Name</label><br>
    <input id="name" type="text" v-model="book.name" placeholder="Name" @change="checkIsActiveButton"><br>

    <label key="language">Language</label><br>
    <input id="language" type="search" list="languages" v-model="book.bookLanguage" placeholder="Language" @change="checkIsActiveButton"><br>
    <datalist id="languages">
      <option v-for="language in languages" :key="language" :value="language">{{ language }}</option>
    </datalist>

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

    <label key="photo">Photo</label>
    <input type="text" v-model="book.imageUrl" @change="checkIsActiveButton"><br>

    <label key="genres">Genres</label><br>
    <div v-for="(genre, index) in book.genres" :key="index">
      <input type="search" list="genres" v-model="book.genres[index]" @input="removeGenreFromList(index)"><br>
      <datalist id="genres">
        <option v-for="choosedGenre in genresList" :key="choosedGenre" :value="choosedGenre">{{ choosedGenre }}</option>
      </datalist>
      <button @click="removeGenre(index)" style="background-color: red; height: auto;" class="main-button">Remove genre</button>
    </div><br>

    <button @click="addGenre" style="background-color: green;" class="main-button">Add genre</button><br>

    <button @click="updateBook" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Update</button>
  </div>

</template>

<script>
import * as bookAPI from "@/js/API/bookAPI";
import * as publisherAPI from "@/js/API/publisherAPI";
import * as authorAPI from "@/js/API/authorAPI";
import * as dateHelper from "@/js/dateHelper";
import * as genresChoices from "@/js/genres";
import * as languages from "@/js/languages";

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
      loaded: false,
      genresList: genresChoices.genres,
      languages: languages.languages
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

    async addGenre() {
      this.book.genres.push("");
    },

    async removeGenreFromList(index) {
      this.genresList = this.genresList.filter(genre => this.book.genres[index] != genre);
    },

    async removeGenre(index) {
      this.genresList.push(this.book.genres[index]);
      this.book.genres.splice(index, 1);
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
        imageUrl: this.book.imageUrl,
        genres: this.book.genres
      };

      await bookAPI.patchUpdateBook(data);
    }
  },

  async mounted() {
    this.publishers = await publisherAPI.getAllPublishers();
    this.authors = await authorAPI.getAllAuthors();
    this.book = await bookAPI.getBook(this.$route.params.id);

    this.book.yearPublication = dateHelper.formatDate(this.book.yearPublication);
    this.authorName = this.authors.find(author => author.id === this.book.authorId).name;
    this.publisherName = this.publishers.find(publisher => publisher.id === this.book.publisherId).name;
    this.genresList = this.genresList.filter(genre => !this.book.genres.includes(genre));
    this.loaded = true;
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>