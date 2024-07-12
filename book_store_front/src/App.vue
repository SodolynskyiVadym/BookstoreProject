<template>
    <header class="header">
        <img :src="require(`@/assets/logo.png`)" style="cursor: pointer;" @click="$router.push('/')">
        <div style="margin-right: 20px;">
            <img :src="require(`@/assets/basket.png`)" class="icons-header" style="padding-right: 30px;" @click="showOrder">
            <img :src="require(`@/assets/home.png`)" v-if="isUser" class="icons-header" style="margin-right: 20px;" @click="enterUserPage">
            <img :src="require(`@/assets/create-author.png`)" v-if="isEditor" class="icons-header" style="margin-right: 20px;" @click="enterCreateAuthorPage">
            <img :src="require(`@/assets/create-book.png`)" v-if="isEditor" class="icons-header" style="margin-right: 20px;" @click="enterCreateBookPage">
            <img :src="require(`@/assets/create-publisher.png`)" v-if="isEditor" class="icons-header" style="margin-right: 20px;" @click="enterCreatePublisherPage">
            <img :src="require(`@/assets/admin.png`)" v-if="isAdmin" class="icons-header" style="margin-right: 20px;" @click="enterAdminPage">
            <img :src="require(`@/assets/registration.png`)" v-if="!isUser" class="icons-header" style="margin-right: 20px;" @click="enterRegistrationPage">
            <img :src="require(`@/assets/login.png`)" class="icons-header" v-if="!isUser" style="margin-right: 20px;" @click="enterLoginPage">
            <img :src="require(`@/assets/logout.png`)" class="icons-header"  v-if="isUser" style="margin-right: 20px;" @click="logout">
        </div>
    </header>

    <div class="my-order" v-if="isShowOrder">
      <p class="order-inscription">Your order</p>
      <div v-for="book in orderedBooks" :key="book.id" class="orderBook">
        <img :src="book.imageUrl" class="image-order">
        <div style="margin-left: 15px; width: 300px;">
          <div style="padding-bottom: 20px;">{{ book.name }}</div>
          <div style="align-self: self-end;">{{ book.authorName }}</div>
        </div>
        <div style="margin-left: 300px; margin-top: 60px; width: 120px;">{{ (book.price - book.price * book.discount / 100).toFixed() }} UAH</div>
        <div style="margin-left: 50px; margin-top: 40px;">
          <div>Quantity: </div>
          <input type="number" :min="1" @change="changeBookQuantity(book)" :max="book.availableQuantity" v-model="book.quantityOrdered" style="width: 80px;">
        </div>
        <button class="button-delete-order" @click="deleteOrderBook(book.id)">Delete</button>
      </div>
      <button class="main-button" style="margin-top: 30px; margin-left: 45%" @click="enterOrderPage">Make order</button>
      <div class="generally-price">Price: {{ generallyPrice }} UAH</div>
    </div>
    <router-view style="z-index: 1;"></router-view>
</template>


<script>
import * as authAPI from "@/js/API/authAPI";
import * as bookAPI from "@/js/API/bookAPI";
import * as orderMaker from "@/js/orderMaker"

export default {
  data() {
    return {
      user: null,
      isShowOrder: false,
      orderedBooks : [],
      generallyPrice : 0,
      isUser: false,
      isAdmin: false,
      isEditor: false
    };
  },

  methods: {
    async calculateGenerallyPrice(){
      this.generallyPrice = 0;
      for(var book of this.orderedBooks){
          this.generallyPrice += (book.price - book.price*book.discount/100) * book.quantityOrdered;
      }
      this.generallyPrice = this.generallyPrice.toFixed();
    },

    async showOrder(){
      if(!this.isShowOrder){
        const orderedBooksId = await orderMaker.getOrderedBooksArray();
        const arrayBookQuantity = await orderMaker.getOrderBookIdQuantity();
        
        console.log(orderedBooksId);
        console.log(arrayBookQuantity);
        this.orderedBooks = await bookAPI.getSomeBook(orderedBooksId);
        console.log(this.orderedBooks);
        for(let book of this.orderedBooks){
          book.quantityOrdered = arrayBookQuantity[book.id];
        }
        await this.calculateGenerallyPrice();
      }
      this.isShowOrder = !this.isShowOrder;
    },

    async deleteOrderBook(id){
      this.orderedBooks = this.orderedBooks.filter(book => book.id !== id);
      const orderLabel = await orderMaker.removeBookFromOrder(id)
      localStorage.setItem("order", orderLabel);
      await this.calculateGenerallyPrice();
    },

    async enterOrderPage(){
      this.isShowOrder = false;
      this.$router.push("/orderPage");
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

    },

    async enterUserPage(){
      this.$router.push('/myPage');
    },

    async enterCreateBookPage(){
      this.$router.push('/createBook');
    },


    async enterCreateAuthorPage(){
      this.$router.push('/createAuthor');
    },

    async enterCreatePublisherPage(){
      this.$router.push('/createPublisher');
    },

    async enterRegistrationPage(){
      this.$router.push('/registration');
    },

    async enterAdminPage(){
      this.$router.push('/adminPage');
    },

    async enterLoginPage(){
      this.$router.push('/login');
    },


    async logout(){
      localStorage.removeItem("token");
      this.$router.push("/");
      setTimeout(() => {
          window.location.reload();
      }, 1);

    }
  },


  async mounted(){
    const token = localStorage.getItem("token");
    if(token){
      this.user = await authAPI.getUserByToken(token);

      if(this.user.role) this.isUser = true;

      if(this.user.role && this.user.role !== "USER") this.isEditor = true;

      if(this.user.role === "ADMIN") this.isAdmin = true;
    }
  }

};
</script>

<style>

@import "@/assets/css/styles.css";
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
  font-size: xx-large;
  color: red;
  border-bottom: solid 2px;
  border-color: black;
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
  margin-top: 30px;
  margin-left: 45%;
  height: 60px;
  width: 250px;
  font-size: x-large;
  font-family:'Gill Sans', 'Gill Sans MT', Calibri, 'Trebuchet MS', sans-serif
}

</style>