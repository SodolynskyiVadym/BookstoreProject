<template>
    <div v-if="loaded">
        <div style="text-align: center;">
            <img class="author-photo" :src="require(`@/assets/authorPhoto/${author.name.toLowerCase().replace(/\s+/g, '')}${author.id}.jpg`)">
            <div style="margin-top: 30px; font-size: x-large;">{{ author.birthYear }}     ----     {{ author.deathYear }}</div>
            <div class="biography">{{ author.biography }}</div>
            <button class="main-button" @click="enterAuthorUpdatePage">Update</button>

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
import * as listUrl from "@/js/listUrl";


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
        const data = await listUrl.requestGetAuthorBooks(this.$route.params.id);
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

.author-photo {
    padding-top: 80px;
}

.biography {
    height: auto;
    margin-top: 50px;
    margin-left: 420px;
    margin-right: 420px;
    border: 2px solid black;
    font-size:x-large;
    border-radius: 13px;
}


.image-order {
  height: 160px;
  width: 90px;
}
</style>