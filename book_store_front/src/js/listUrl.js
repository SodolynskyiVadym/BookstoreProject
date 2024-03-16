import axios from "axios";

const mainUrl = "http://localhost:5284";


export async function requestGetBooks(){
    return await axios.get(mainUrl + "/book/getAllBooks").then(res => res.data);
}


export async function requestGetBook(id){
    return await axios.get(mainUrl + "/book/getBook/" + id).then(res => res.data);
}

export async function requestGetSomeBook(arrayBooks){
    return await axios.post(mainUrl + "/book/getSomeBooks/", arrayBooks).then(res => res.data);
}

export async function requestGetAuthorBooks(id){
    return await axios.get(mainUrl + "/book/getAuthorBooks/" + id).then(res => res.data);
}