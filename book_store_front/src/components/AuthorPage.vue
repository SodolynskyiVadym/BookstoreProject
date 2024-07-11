<template>
    <div v-if="loaded">
        <div style="text-align: center;">
            <img class="author-photo" :src="author.imageUrl">
            <div style="margin-top: 30px; font-size: x-large;">{{ author.birthYear }}     ----     {{ author.deathYear }}</div>
            <div class="biography">{{ author.biography }}</div>
            <button class="main-button" @click="enterAuthorUpdatePage">Update</button>

            <div v-for="book in books" :key="book.id" class="list-book">
                <img :src="book.imageUrl" class="image-order">
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
import * as authorAPI from '@/js/API/authorAPI';


export default {
    data(){
        return{
            loaded: false,
            books: [],
            author: null
        }
    },

    methods: {
        async enterBookView(id){
            this.$router.push(`/bookView/${id}`);
        },


        async enterAuthorUpdatePage(){
            this.$router.push(`/updateAuthor/${this.$route.params.id}`);
        }
    },

    async mounted(){
        const data = await authorAPI.getAuthorBooks(this.$route.params.id);
        this.author = data.author;
        this.books = data.books;
        this.author.birthYear = this.author.birthYear != null ? this.author.birthYear.split('T')[0] : "???";
        this.author.deathYear = this.author.deathYear != null ? this.author.deathYear.split('T')[0] : "???"; 
        this.loaded = true;
    }
}
</script>


<style scoped>
@import "@/assets/css/styles.css";
@import "@/assets/css/authorPage.css";
</style>