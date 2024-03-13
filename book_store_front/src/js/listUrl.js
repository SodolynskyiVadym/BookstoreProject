import axios from "axios";

const mainUrl = "http://localhost:5284";


export async function requestGetBooks(){
    return await axios.get(mainUrl + "/book/getAllBooks").then(res => res.data);
}


export async function requestGetBook(id){
    return await axios.get(mainUrl + "/book/getInfoBook/" + id).then(res => res.data);
}