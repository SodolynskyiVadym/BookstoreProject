<template>
  <div style="padding-top: 100px; text-align: center">
    <input placeholder="Searth the book or author..." type="search" list="book-author" name="text" class="search-input" v-model="inputValue">
    <datalist id="book-author">
      <option v-for="book in books" :key="book.id" :value="book.name"></option>
      <option v-for="author in authors" :key="author.id" :value="author.name"></option>
    </datalist><br>
    <a class="btn" @click="enterSearchedPage">Find</a>
  </div>


  <div class="book-section">

    <div v-for="book in books" :key="book.id" class="book">
      <a @click="enterBookPage(book.id)">
        <img class="img-book" :src="book.imageUrl">
        <div class="book-name">{{ book.name }}</div>
        <p v-if="book.discount > 0 && book.discount <= 100">
          <span class="full-price">{{ book.price }} UAH</span>
          <sup>-{{ book.discount }}%</sup>
        </p>
        <div>{{ (book.price - (book.price * book.discount / 100)).toFixed(0) }} UAH</div>
      </a>
      <div v-if="book.availableQuantity > 0">
        <button class="button-buy" v-if="!book.isOrdered" @click="buyBook(book)"
          :disabled="book.availableQuantity <= 0">Buy</button>
        <button style="background-color: gray; width: auto; cursor: auto;" class="button-buy" disabled v-else>Already in
          your backet</button>
      </div>
      <div v-else>
        <button style="background-color: gray; width: auto; cursor: auto;" class="button-buy" disabled>Not available</button>
      </div>
    </div>
  </div>
</template>


<script>
import * as bookAPI from "@/js/API/bookAPI";
import * as authorAPI from "@/js/API/authorAPI";
import * as orderMaker from "@/js/orderMaker"

export default {
  data() {
    return {
      books: [],
      authors: [],
      inputValue: "",
      showModal: false
    };
  },

  methods: {
    enterSearchedPage() {
      const book = this.books.find(b => b.name === this.inputValue);
      if(book) {
        this.$router.push(`/bookView/${book.id}`);
      }
      
      const author = this.authors.find(a => a.name === this.inputValue);
      if(author) {
        this.$router.push(`/author/${author.id}`);
      }
    },

    async enterBookPage(id) {
      this.$router.push(`/bookView/${id}`);
    },

    async buyBook(book) {
      await orderMaker.addBookToOrder(book.id, 1);
      book.isOrdered = true;
    }
  },

  async mounted() {
    this.authors = await authorAPI.getAllAuthors();
    this.books = await bookAPI.getBooks();
    var indexesOrderedBooks = await orderMaker.getOrderedBooksArray();

    for (var book of this.books) {
      if (indexesOrderedBooks.includes(book.id)) book.isOrdered = true;
    }
  }
};
</script>



<style>
@import "@/assets/css/mainPage.css";
@import "@/assets/css/search-input.css";
</style>