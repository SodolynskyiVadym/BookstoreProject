<template>
  <div v-if="loaded">
    <div class="author-nameBook">
      <div>{{ book.authorName }}</div>
      <div>Book "{{book.name}}"</div>
    </div>
    <div class="book-info">
      <img class="img-book" :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)">
      <div class="book-info-section" style="border-top-left-radius: 15px; border-bottom-left-radius: 15px;">
        <br><br><br>
        <div>Name ----------------</div>
        <div>Author --------------</div>
        <div>Year publication ----</div>
        <div>Pages ---------------</div>
        <div>Language ------------</div>
        <div>Publisher -----------</div>
        <div>Price ---------------</div>
        <div>Discount ------------</div>
      </div>
      <div class="book-info-section" style="margin-left: 0px; border-bottom-right-radius: 15px; border-top-right-radius: 15px;">
        <div class="isBookStyle">THE BOOK IS IN STOCK</div>
        <div>{{ book.name }}</div>
        <a @click="enterAuthorPage"><div>{{ book.authorName }}</div></a>
        <div>{{ book.yearPublication}}</div>
        <div>{{ book.numberPages }}</div>
        <div>{{ book.bookLanguage }}</div>
        <div>{{ book.publisherId }}</div>
        <div>{{ book.price }}</div>
        <div>{{ book.discount }}</div>
      </div>
      <div class="buy-section">
        <div style="margin: 15px;">{{ book.authorName + " - <" + book.name + ">" }}</div>
        <br><br><br>
        <div style="display: flex; justify-content: space-between;">
          <div style="margin-left: 30px;">Price</div>
          <div style="text-align: right; margin-right: 30px;">{{ (book.price - (book.price * book.discount / 100)).toFixed(0) }} UAH</div>
        </div>

        <div style="display: flex; justify-content: space-between; margin-top: 20px;">
          <input type="number" style="margin-left: 30px; font-size: large;" min="1" :max="book.availableQuantity" value="1">
          <button class="button-buy">Buy</button>
        </div>
      </div>
    </div>
    <div class="description">
      <text>
      {{ book.description }}
      </text>
    </div>
  </div>
</template>


<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
    return {
      book: null,
      loaded: false
    };
  },

  methods: {
    async enterAuthorPage(){
      this.$router.push(`/author/${this.book.authorId}`);
    }
  },

  async mounted() {
    this.book = await listURL.requestGetBook(this.$route.params.id);
    this.loaded = true;
  }
}
</script>

<style>

.author-nameBook {
  margin-left: 100px;
  font-size: x-large;
  font-weight: bold;
  font-family: Georgia, 'Times New Roman', Times, serif;
}

.book-info {
  margin-left: 200px;
  margin-top: 50px;
  display: flex;
}

.book-info-section {
  margin-left: 40px;
  background-color: rgb(218, 217, 217);
  height: 400px;
}

.book-info-section div {
  margin: 14px;
  font-weight: medium;
  font-family: Georgia, 'Times New Roman', Times, serif;
  font-size: x-large;
}

.isBookStyle {
  color: green;
  padding-bottom: 10px;
}


.buy-section {
  box-shadow: 10px 10px 10px 10px;
  margin-left: 40px;
  font-weight: medium;
  font-family: Georgia, 'Times New Roman', Times, serif;
  font-size: x-large;
  height: 300px;
  width: 400px;
}


.button-buy {
    color: white;
    width: 100px;
    height: 50px;
    background-color: red;
    border-radius: 15px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1.2em;
    border: none;
    text-align: center; 
    margin-right: 30px;
}

.button-buy:hover {
    background-color: brown;
}

.description {
  height: auto;
  margin-top: 50px;
  margin-left: 420px;
  margin-right: 420px;
  border: 2px solid black;
  font-size:x-large;
  border-radius: 13px;
}

</style>