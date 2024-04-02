<template>
  <div class="main-section" style="margin-bottom: 100px">
    <div class="generally-price">Price: {{generallyPrice}} UAH</div>
    <label key="destination">Destination</label><br>
    <input type="text" v-model="destination" id="destination" placeholder="Destination" @change="checkIsActive"><br>
    <label key="phoneNumber">Phone number write as xxx-xxx-xxxx</label><br>
    <input type="text" v-model="phoneNumber" id="phoneNumber" placeholder="Phone number" @change="checkIsActive"><br>
    <button :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive" @click="makeOrder">Buy</button>
  </div>

  <div v-for="book in orderedBooks" :key="book.id" class="list-book">
    <img :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)" class="image-order">
    <div style="margin-left: 15px; width: 300px;">
      <div style="margin-top: 20%;">{{ book.name }}</div>
    </div>
    <div style="margin-left: 300px; margin-top: 60px; width: 120px;">{{ ((book.price - book.price * book.discount  / 100)*book.quantityOrdered).toFixed() }} UAH</div>
    <input type="number" class="quantityBookOrder" style="width: 80px" v-model="book.quantityOrdered" min="1" :max="book.availableQuantity" @change="changeBookQuantity(book)">
    <button class="button-delete-order" @click="deleteOrder(book.id)">Delete</button>
  </div>
</template>

<script>
import * as listURL from "@/js/listUrl";
import * as orderMaker from "@/js/orderMaker";

export default {
  data() {
    return {
      destination: "",
      orderedBooks: [],
      isLoaded: false,
      phoneNumber: "",
      generallyPrice: 0,
      isActive: false
    }
  },

  methods: {
    async checkIsPhoneNumber(phoneNumber){
      const phoneNumberPattern = /^(\()?\d{3}(\))?(-|\s)?\d{3}(-|\s)\d{4}$/;
      return phoneNumberPattern.test(phoneNumber);
    },

    async checkIsActive(){
      let flag = true;
      for (let book of this.orderedBooks){
        if (book.orderedQuantity < 1 || book.orderedQuantity > book.availableQuantity){
          flag = false;
          break;
        }
      }
      this.isActive = flag && this.orderedBooks.length > 0 && this.destination && await this.checkIsPhoneNumber(this.phoneNumber);

    },


    async calculateGenerallyPrice() {
      await this.checkIsActive();
      this.generallyPrice = 0;
      for (let book of this.orderedBooks) {
        this.generallyPrice += (book.price - book.price * book.discount / 100) * book.quantityOrdered;
      }
      this.generallyPrice = this.generallyPrice.toFixed();
    },


    async deleteOrder(id){
      this.orderedBooks = this.orderedBooks.filter(book => book.id !== id);
      await orderMaker.removeBookFromOrder(id);
      await this.calculateGenerallyPrice();
    },


    async makeOrder(){

    },

    async changeBookQuantity(book){
      if(book.quantityOrdered === 0)
      {
        book.quantityOrdered = 1;
        await orderMaker.changeQuantity(book.id, book.quantityOrdered);
      }
      else{
        await orderMaker.changeQuantity(book.id, book.quantityOrdered);
      }
      await this.calculateGenerallyPrice();
      await this.checkIsActive();

    },
  },


    async mounted() {
      const orderedBooksId = await orderMaker.getOrderedBooksArray();
      const arrayBookQuantity = await orderMaker.getOrderBookIdQuantity();
      this.orderedBooks = await listURL.requestGetSomeBook(orderedBooksId);
      console.log(this.orderedBooks)
      for (let book of this.orderedBooks) {
        book.quantityOrdered = arrayBookQuantity[book.id];
      }
      await this.calculateGenerallyPrice();
      await this.checkIsActive()
    }
}
</script>

<style>
@import "@/assets/css/styles.css";

.quantityBookOrder{
  margin-top: 40px;
  margin-left: 100px
}

.button-delete-order {
  margin-top: 60px;
  margin-left: 30px;
  background-color: red;
  color: white;
  width: 100px;
  height: 50px;
  text-align: center;
  border-radius: 15px;
  cursor: pointer;
  font-weight: blod;
  font-size: 1.2em;
  border: none;
}

.button-delete-order:hover{
  background-color: brown;
}


.generally-price {
  font-size: xx-large;
  font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif
}
</style>