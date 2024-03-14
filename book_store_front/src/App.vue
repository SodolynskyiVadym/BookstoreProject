<template>
    <header class="header">
        <img :src="require(`@/assets/logo.png`)" >
        <div style="margin-right: 20px;">
            <img :src="require(`@/assets/basket.png`)" class="icons-header" style="padding-right: 30px;" @click="showButton">
            <img :src="require(`@/assets/user.png`)" class="icons-header" style="margin-right: 20px;">
        </div>
    </header>
    <div class="my-order" v-if="showModal">Hello</div>
    <router-view style="z-index: 1;"></router-view>
</template>


<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
    return {
      showModal: false
    };
  },

  methods: {
    arrayToKeyValue(array) {
        const result = {};
        for (let i = 0; i < array.length; i += 2) {
            result[array[i]] = array[i + 1];
            }
        return result;
        },


    async showButton(){
      this.showModal = !this.showModal;
    //   localStorage.setItem("order", "1 2 3 4 6 6");
      
    //   console.log(localStorage.getItem("order").split(" "));
    //   const array = this.arrayToKeyValue(localStorage.getItem("order").split(" "));
    //   console.log(await listURL.requestGetSomeBook(array));
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
  background-color: white;
  width: 50%;
}
</style>