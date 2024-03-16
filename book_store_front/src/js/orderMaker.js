export async function getOrderedBooksArray(){
    const result = [];
    if(localStorage.getItem("order")){
      const array = localStorage.getItem("order").split(" ");
      for (let i = 0; i < array.length - 2; i += 2) {
        result.push(parseInt(array[i]));
      }
    }
    return result;
}


export async function getOrderBookQuantity(){
    const result = {};
    if(localStorage.getItem("order")){
        const array = localStorage.getItem("order").split(" ");
        for (let i = 0; i < array.length - 2; i += 2) {
            result[array[i]] = array[i + 1];
        }
    }
    return result;
}


export async function removeBookFromOrder(id){
    var strOrder = "";
    if(localStorage.getItem("order")){
        const array = localStorage.getItem("order").split(" ");
        for (let i = 0; i < array.length - 2; i += 2) {
            if(array[i] != id) strOrder = strOrder + `${array[i]} ${array[i+1]} `
        }

        localStorage.setItem("order", strOrder);
    }
    return strOrder;
}


export async function changeQuantity(id, quantity){
    var strOrder = "";
    const array = localStorage.getItem("order").split(" ");
    for (let i = 0; i < array.length - 2; i += 2) {
        if(array[i] != id) strOrder = strOrder + `${array[i]} ${array[i+1]} `;
        else strOrder = strOrder + `${array[i]} ${quantity} `;
    }
    localStorage.setItem("order", strOrder);
}


export async function addBookToOrder(id, quantity){
    if(localStorage.getItem("order")) localStorage.setItem("order", localStorage.getItem("order") + `${id} ${quantity} `);
    else localStorage.setItem("order", `${id} ${quantity} `);
}