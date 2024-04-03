<template>
  <div>Clean up order</div>
</template>

<script>

export default {
  data() {
    return {

    }
  },

  methods: {

  },

  async mounted() {
    localStorage.removeItem("order");
    console.log("Order cleaned");
    this.$router.push("/");
  }
}
</script>