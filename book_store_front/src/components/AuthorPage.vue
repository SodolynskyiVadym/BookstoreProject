<template>
    <div v-if="loaded">
        <div>Hello</div>
        <div style="text-align: center;">
            <img class="author-photo" :src="require(`@/assets/authorPhoto/${author.name.toLowerCase().replace(/\s+/g, '')}${author.id}.jpg`)">
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

    async mounted(){
        const data = await listUrl.requestGetAuthorBooks(this.$route.params.id);
        this.author = data.author;
        this.books = data.books;
        this.loaded = true;
    }
}
</script>


<style scoped>
.author-photo {
    padding-top: 80px;
}

</style>