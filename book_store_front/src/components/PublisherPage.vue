<template>
    <div v-if="loaded">
        <div style="text-align: center;">
            <img class="publisher-photo" :src="require(`@/assets/publisherPhoto/${publisher.name.toLowerCase().replace(/\s+/g, '')}${publisher.id}.jpg`)"><br>

            <button class="main-button" @click="enterPublisherUpdatePage">Update</button>

            <div v-for="book in books" :key="book.id" class="list-book">
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
import * as publisherAPI from '@/js/API/publisherAPI';


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
        const data = await publisherAPI.getPublisherBooks(this.$route.params.id);
        this.publisher = data.publisher;
        this.books = data.books;
        this.loaded = true;
    }
}
</script>


<style scoped>
@import "@/assets/css/styles.css";
.publisher-photo {
    padding-top: 80px;
}


.image-order {
  height: 160px;
  width: 90px;
}
</style>