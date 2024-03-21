import axios from "axios";

const mainUrl = "http://localhost:5284";


export async function requestGetBooks(){
    return await axios.get(mainUrl + "/book/getAllBooks").then(res => res.data);
}


export async function requestGetBook(id){
    return await axios.get(mainUrl + "/book/getBook/" + id).then(res => res.data);
}

export async function requestGetSomeBook(arrayBooks){
    const strBooksId = arrayBooks.join(",");
    return await axios.get(mainUrl + "/book/getSomeBooks?ids=" +  strBooksId).then(res => res.data);
}

export async function requestGetAuthorBooks(id){
    return await axios.get(mainUrl + "/book/getAuthorBooks/" + id).then(res => res.data);
}


export async function requestGetPublisherBooks(id){
    return await axios.get(mainUrl + "/book/getPublisherBooks/" + id).then(res => res.data);
}


export async function requestGetAuthor(id){
    return await axios.get(mainUrl + "/book/getAuthor/" + id).then(res => res.data);
}


export async function requestGetAllAuthors(){
    return await axios.get(mainUrl + "/book/getAllAuthors").then(res => res.data);
}


export async function requestGetAllPublishers(){
    return await axios.get(mainUrl + "/book/getAllPublishers").then(res => res.data);
}


export async function requestPostCreateBook(data){
    return await axios.post(mainUrl + "/book/createBook", data).then(res => res.data)
}


export async function requestPostCreateAuthor(data){
    return await axios.post(mainUrl + "/book/createAuthor", data).then(res => res.data)
}


export async function requestPatchUpdateBook(data){
    return await axios.patch(mainUrl + "/book/updateBook", data).then(res => res.data)
}


export async function requestPatchUpdateAuthor(data){
    return await axios.patch(mainUrl + "/book/updateAuthor", data).then(res => res.data)
}