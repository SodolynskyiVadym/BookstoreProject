<template>
    <header class="header">
        <img :src="require(`@/assets/logo.png`)" >
        <div style="margin-right: 20px;">
            <img :src="require(`@/assets/basket.png`)" class="icons-header" style="padding-right: 30px;" @click="showOrder">
            <img :src="require(`@/assets/user.png`)" class="icons-header" style="margin-right: 20px;">
        </div>
    </header>

    <div class="my-order" v-if="isShowOrder">
      <p class="order-inscription">Your order</p>
      <div v-for="book in orderedBook" :key="book.id" class="orderBook">
        <img :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)" class="image-order">
        <div style="margin-left: 15px; width: 300px;">
          <div style="padding-bottom: 20px;">{{ book.name }}</div>
          <div style="align-self: self-end;">{{ book.authorName }}</div>
        </div>
        <div style="margin-left: 350px; margin-top: 40px; width: 120px;">{{ (book.price - book.price * book.discount / 100).toFixed() }} UAH</div>
        <div style="margin-left: 50px; margin-top: 40px;">
          <div>Quantity: </div>
          <input type="number" min="1" v-model="book.quantityOrdered">
        </div>
      </div>
    </div>
    <router-view style="z-index: 1;"></router-view>
</template>


<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
    return {
      isShowOrder: false,
      orderedBook : []
    };
  },

  methods: {
    orderStringToArray(array) {
        const result = {};
        for (let i = 0; i < array.length - 2; i += 2) {
            result[array[i]] = array[i + 1];
            }
        return result;
        },


    async showOrder(){
      
      const array = this.orderStringToArray(localStorage.getItem("order").split(" "));
      this.orderedBook = await listURL.requestGetSomeBook(array);
      for(var book of this.orderedBook){
        book.quantityOrdered = array[book.id];
      }

      this.isShowOrder = !this.isShowOrder;
    }
  },

};
</script>

<style>


.header {
    z-index: 2;
    width: 100%;
    height: 80px;
    background-color: black;
    display: flex;
    justify-content:space-between;
    position: fixed;
}

.icons-header {
    width: 60px;
    height: 60px;
    margin-top: 10px;
    cursor: pointer;
}


.my-order {
  z-index: 3;
  margin-top: 200px;
  position: fixed;
  margin-left: 500px;
  height: 600px;
  background-color: rgb(197, 197, 197);
  width: 60%;
  border-radius: 15px;
  border: 2px solid black;
  overflow-y: scroll;
}

.order-inscription {
  font-size: x-large;
  color: rgb(34, 177, 41);
  background-color: rgb(105, 105, 105);
}

.orderBook {
  display: flex;
  padding-bottom: 10px;
  padding-top: 10px;
  padding-left: 10px;
  border-bottom: solid 1px;
  border-top: solid 1px
}

.orderBook div{
  font-size: x-large;
  font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif
}

.image-order {
  height: 160px;
  width: 90px;
}

</style>