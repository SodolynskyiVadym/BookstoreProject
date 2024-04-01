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
        <div class="isBookStyle" v-if="isInStock">THE BOOK IS IN STOCK</div>
        <div class="isNotBookStyle" v-else>THE BOOK IS NOT IN STOCK</div>
        <div>{{ book.name }}</div>
        <a @click="enterAuthorPage" style="color: blue; cursor: pointer; text-decoration: underline;"><div>{{ book.authorName }}</div></a>
        <div>{{ book.yearPublication}}</div>
        <div>{{ book.numberPages }}</div>
        <div>{{ book.bookLanguage }}</div>
        <div @click="enterPublisherPage" style="color: blue; cursor: pointer; text-decoration: underline;">{{ book.publisherName }}</div>
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
          <input type="number" v-if="isInStock" style="margin-left: 30px; font-size: large;" min="1" :max="book.availableQuantity" v-model="orderedQuantity" :value="orderedQuantity" @change="changeQuantityOrder()">
          <button class="button-buy" @click="enterBookUpdatePage">Update</button>
          <div v-if="isInStock">
            <button v-if="isNoOrdered" @click="changeOrCreateOrder(book.id)" class="button-buy">Buy</button>
            <button v-else class="button-buy" @click="cancelOrder(book.id)">Cancel</button>
          </div>
          

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
import * as orderMaker from "@/js/orderMaker"

export default {
  data() {
    return {
      book: null,
      loaded: false,
      firstOrderedQuantity: 0,
      orderedQuantity: 0,
      isNoOrdered: false,
      isInStock: false
    };
  },

  methods: {
    async enterAuthorPage(){
      this.$router.push(`/author/${this.book.authorId}`);
    },

    async formatDate(date) {
      const d = new Date(date);
      const day = d.getDate().toString().padStart(2, '0');
      const month = (d.getMonth() + 1).toString().padStart(2, '0');
      const year = d.getFullYear();
      return `${year}-${month}-${day}`;
    },


    async cancelOrder(id){
      await orderMaker.removeBookFromOrder(id);
      this.firstOrderedQuantity = -1;
      this.isNoOrdered = true;
    },

    async changeOrCreateOrder(id){
      const arrayOrderedBooks = await orderMaker.getOrderedBooksArray();
      if(arrayOrderedBooks.includes(id)){
        await orderMaker.changeQuantity(id, this.orderedQuantity);
      }
      else await orderMaker.addBookToOrder(id, this.orderedQuantity);

      this.firstOrderedQuantity = this.orderedQuantity;
      this.isNoOrdered = false;

      

    },

    async changeQuantityOrder(){
      this.isNoOrdered = this.firstOrderedQuantity !== this.orderedQuantity;
    },


    async enterBookUpdatePage(){
      this.$router.push(`/updateBook/${this.$route.params.id}`);
    },


    async enterPublisherPage(){
      this.$router.push(`/publisher/${this.book.publisherId}`);
    }
  },

  async mounted() {
    this.book = await listURL.requestGetBook(this.$route.params.id);
    const orderBookQuantity = await orderMaker.getOrderBookQuantity();
    const orderedBooks = await orderMaker.getOrderedBooksArray();

    this.orderedQuantity = orderBookQuantity[this.book.id] ?? 1;
    this.firstOrderedQuantity = orderBookQuantity[this.book.id] ?? -1;

    this.isNoOrdered = !orderedBooks.includes(this.book.id);
    this.isInStock = this.book.availableQuantity > 0;
    this.book.yearPublication = await this.formatDate(this.book.yearPublication);
    this.loaded = true;
    console.log(this.book.availableQuantity)
  }
}
</script>

<style>

.author-nameBook {
  padding-top: 100px;
  margin-left: 100px;
  font-size: x-large;
  font-weight: bold;
  font-family: Georgia, 'Times New Roman', Times, serif;
}

.book-info {
  margin-left: 200px;
  margin-top: 20px;
  display: flex;
}

.book-info-section {
  margin-left: 40px;
  background-color: rgb(218, 217, 217);
  height: 400px;
}

.book-info-section div {
  margin: 14px;
  font-family: Georgia, 'Times New Roman', Times, serif;
  font-size: x-large;
}

.isBookStyle {
  color: green;
  padding-bottom: 10px;
}

.isNotBookStyle {
  color: rgb(255, 0, 0);
  padding-bottom: 10px;
}


.buy-section {
  box-shadow: 10px 10px 10px 10px;
  margin-left: 40px;
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
    font-size: 1.2em;
    border: none;
    text-align: center; 
    margin-right: 10px;
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