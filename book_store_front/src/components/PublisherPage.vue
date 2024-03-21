<template>
    <div v-if="loaded">
        <div style="text-align: center;">
            <img class="publisher-photo" :src="require(`@/assets/publisherPhoto/${publisher.name.toLowerCase().replace(/\s+/g, '')}${publisher.id}.jpg`)"><br>

            <button class="button-update" @click="enterPublisherUpdatePage">Update</button>

            <div v-for="book in books" :key="book.id" class="publisher-book">
                <img :src="require(`@/assets/bookPhoto/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)" class="image-order">
                <div style="margin-left: 15px; width: 300px;">
                <div style="margin-top: 20%;">{{ book.name }}</div>
                </div>
                <div style="margin-left: 300px; margin-top: 60px; width: 120px;">{{ (book.price - book.price * book.discount / 100).toFixed() }} UAH</div>
                <button class="button-view" @click="enterBookView(book.id)">View</button>
                <div style="margin-left: 50px; margin-top: 40px;">
                </div>
            </div>
        </div>

    </div>
</template>

<script>
import * as listUrl from "@/js/listUrl";


export default {
    data(){
        return{
            loaded: false,
            books: [],
            publisher: null
        }
    },

    methods: {
        async enterBookView(id){
            this.$router.push(`/bookView/${id}`);
        },


        async enterPublisherUpdatePage(){
            this.$router.push(`/updatePublisher/${this.$route.params.id}`);
        }
    },

    async mounted(){
        const data = await listUrl.requestGetPublisherBooks(this.$route.params.id);
        this.publisher = data.publisher;
        this.books = data.books;
        this.loaded = true;
    }
}
</script>


<style scoped>
.publisher-photo {
    padding-top: 80px;
}


.publisher-book {
  display: flex;
  padding-bottom: 10px;
  padding-top: 10px;
  padding-left: 10px;
  margin-left: 180px;
  margin-right: 280px;
  margin-top: 20px;
  border: solid 1px;
}

.publisher-book div{
    font-size: x-large;
}


.image-order {
  height: 160px;
  width: 90px;
}

.button-view {
  margin-top: 50px;
  margin-left: 250px;
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

.button-view:hover{
  background-color: brown;
}


.button-update {
  color: white;
  position: center;
  width: 100px;
  height: 50px;
  text-align: center;
  background-color: rgb(0, 0, 0);
  border-radius: 15px;
  cursor: pointer;
  font-weight: blod;
  font-size: 1.2em;
  border: none;
  margin-top: 20px;
}

.button-update:hover {
  background-color: rgb(48, 45, 45);
}
</style>