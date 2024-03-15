<template>
  <div class="book-section">
    <div v-for="book in books" :key="book.id" class="book">
      <a @click="enterBookPage(book.id)">
        <img class="img-book" :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)">
        <div class="book-name">{{ book.name }}</div>
        <p>
            <span class="full-price">{{ book.price }} UAH</span>
            <sup>-{{ book.discount }}%</sup>
        </p>
        <div>{{ (book.price - (book.price * book.discount / 100)).toFixed(0) }} UAH</div>
      </a>
      <button class="button-buy" v-if="!book.isOrdered" @click="buyBook(book)">Buy</button>
      <button style="background-color: gray; width: auto; cursor: auto;" class="button-buy" v-else >Already in your backet</button>
    </div>
  </div>
</template>
  

<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
    return {
      books: [],
      showModal: false
    };
  },

  methods: {
    enterBookPage(bookId){
      this.$router.push(`/bookView/${bookId}`)
    },

    async orderStringToIntArray() {
      const result = [];
      if(localStorage.getItem("order")){
        const array = localStorage.getItem("order").split(" ");
        for (let i = 0; i < array.length - 2; i += 2) {
          result.push(parseInt(array[i]));
        }
      }
      return result;
    },


    buyBook(book){
      if(localStorage.getItem("order")) localStorage.setItem("order", localStorage.getItem("order") + `${book.id} 1 `);
      else localStorage.setItem("order", `${book.id} 1 `);
      book.isOrdered = true;
    }
  },

  async mounted() {
    this.books = await listURL.requestGetBooks();
    var indexesOrderedBooks = await this.orderStringToIntArray();
    
    for(var book of this.books){
      if(indexesOrderedBooks.includes(book.id)) {
        book.isOrdered = true;
      }
    }
  }
};
</script>



<style>
.book-section {
  z-index: 1;
  padding-top: 80px;
  margin-left: 100px;
  margin-right: 100px;
  display: flex;
  flex-wrap: wrap;
}

.book {
    margin: 30px;    
    width: 300px;
    padding: 10px;
    text-align: center;
}

.img-book {
    width: 300px;
    height: 500px;
}

.book-name{
    font-family:Georgia, 'Times New Roman', Times, serif;
    font-size: large;
}

.button-buy {
    color: white;
    width: 100px;
    height: 50px;
    text-align: center;
    background-color: red;
    border-radius: 15px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1.2em;
    border: none;
}

.button-buy:hover {
    background-color: brown;
}

.full-price {
    text-decoration: line-through;
}

.book:hover {
    cursor: alias;
    background-color:darkgrey;
    transform: scale(1.1);
    transition: transform 0.5s ease;
}
</style>