<template>
    <div class="book-section">
      <div v-for="book in books" :key="book.id" class="book">
            <img class="img-book" :src="require(`@/assets/${book.name.toLowerCase().replace(/\s+/g, '')}${book.id}.jpg`)">
            <div class="book-name">{{ book.name }}</div>
            <p>
                <span class="full-price">{{ book.price }} UAH</span>
                <sup>-{{ book.discount }}%</sup>
            </p>
            <div>{{ (book.price - (book.price * book.discount / 100)).toFixed(0) }} UAH</div>
            <button class="button-buy">Buy</button>
      </div>
    </div>
</template>
  

<script>
import * as listURL from "@/js/listUrl";

export default {
  data() {
    return {
      books: [],
      userID: "",
      userGroupsID: []
    };
  },

  async mounted() {
    this.books = await listURL.requestGetBooks();
  }
};
</script>



<style>
.book-section {
  margin-left: 100px;
  margin-right: 100px;
  display: flex;
  flex-wrap: wrap;
}

.book {
    margin: 30px;    
    width: 300px; /* Займати 1/3 ширини контейнера */
    padding: 10px;
    text-align: center;
}

.img-book {
    width: 300px;
    height: 500px;
}

.book-name{
    font-family:Georgia, 'Times New Roman', Times, serif;
    font-size: large;
}

.button-buy {
    color: white;
    width: 100px;
    height: 50px;
    text-align: center;
    background-color: red;
    border-radius: 15px;
    cursor: pointer;
    font-weight: blod;
    font-size: 1.2em;
    border: none;
}

.button-buy:hover {
    background-color: brown;
}

.full-price {
    text-decoration: line-through;
}

.book:hover {
    cursor: alias;
    background-color:darkgrey;
    transform: scale(1.1);
    transition: transform 0.5s ease;
}
</style>