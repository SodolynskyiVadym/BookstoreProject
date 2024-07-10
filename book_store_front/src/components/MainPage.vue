<template>
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
        <button class="button-buy" v-if="!book.isOrdered" @click="buyBook(book)" :disabled="book.availableQuantity <= 0">Buy</button>
        <button style="background-color: gray; width: auto; cursor: auto;" class="button-buy" disabled v-else >Already in your backet</button>
      </div>
      <div v-else>
        <button style="background-color: gray; width: auto; cursor: auto;" class="button-buy" disabled>Not available</button>
      </div>
    </div>
  </div>
</template>
  

<script>
import * as bookAPI from "@/js/API/bookAPI";
import * as orderMaker from "@/js/orderMaker"

export default {
  data() {
    return {
      books: [],
      showModal: false
    };
  },

  methods: {
    enterBookPage(bookId){
      this.$router.push(`/bookView/${bookId}`);
    },

    async buyBook(book){
      await orderMaker.addBookToOrder(book.id, 1);
      book.isOrdered = true;
    }
  },

  async mounted() {
    this.books = await bookAPI.getBooks();
    var indexesOrderedBooks = await orderMaker.getOrderedBooksArray();
    
    for(var book of this.books){
      console.log(book);
      console.log(book.imageUrl);
      if(indexesOrderedBooks.includes(book.id)) {

        book.isOrdered = true;
      }
    }
  }
};
</script>



<style>
@import "@/assets/css/mainPage.css";
</style>