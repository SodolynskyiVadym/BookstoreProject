import axios from "axios";

const mainUrl = "http://localhost:5224";



export async function requestGetUserByToken(token) {
  const config = {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  };

  return await axios.get(mainUrl + "/auth/getUserByToken", config).then(res => res.data);
}



export async function requestGetBooks(){
    return await axios.get(mainUrl + "/book/getAllBooks").then(res => res.data);
}


export async function requestGetPublisher(id){
    return await axios.get(mainUrl + "/publisher/getPublisher/" + id).then(res => res.data);
}


export async function requestGetBook(id){
    return await axios.get(mainUrl + "/book/getBook/" + id).then(res => res.data);
}

export async function requestGetSomeBook(arrayBooks){
    const strBooksId = arrayBooks.join(",");
    return await axios.get(mainUrl + "/book/getSomeBooks?ids=" +  strBooksId).then(res => res.data);
}

export async function requestGetAuthorBooks(id){
    return await axios.get(mainUrl + "/author/getAuthorBooks/" + id).then(res => res.data);
}


export async function requestGetPublisherBooks(id){
    return await axios.get(mainUrl + "/publisher/getPublisherBooks/" + id).then(res => res.data);
}


export async function requestGetAuthor(id){
    return await axios.get(mainUrl + "/author/getAuthor/" + id).then(res => res.data);
}


export async function requestGetAllAuthors(){
    return await axios.get(mainUrl + "/author/getAllAuthors").then(res => res.data);
}


export async function requestGetAllPublishers(){
    return await axios.get(mainUrl + "/publisher/getAllPublishers").then(res => res.data);
}


export async function requestGetAllUsers(){
    return await axios.get(mainUrl + "/auth/getAllUsers").then(res => res.data);
}


export async function requestPostCreateBook(data, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };

    return await axios.post(mainUrl + "/book/createBook", data, config).then(res => res.data)
}


export async function requestPostCreateAuthor(data){
    return await axios.post(mainUrl + "/author/createAuthor", data).then(res => res.data)
}


export async function requestPatchUpdateUser(data, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };
    return await axios.patch(mainUrl + "/auth/updateUser", data, config).then(res => res.data)
}

export async function requestPostRegistration(data){
    return await axios.post(mainUrl + "/auth/registerUser", data).then(res => res.data)
}


export async function requestPostLogin(data){
    return await axios.post(mainUrl + "/auth/login", data).then(res => res.data)
}


export async function requestPostRegistrationWorker(data, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };
    return await axios.post(mainUrl + "/auth/registerWorker", data, config).then(res => res.data);
}


export async function requestCreateBookImage(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    return await axios.post(mainUrl + "/book/createImageBook", data, config).then(res => res.data);
}


export async function requestPatchUpdateBook(data){
    return await axios.patch(mainUrl + "/book/updateBook", data).then(res => res.data)
}


export async function requestPatchUpdateAuthor(data){
    return await axios.patch(mainUrl + "/author/updateAuthor", data).then(res => res.data)
}


export async function requestPatchUpdatePublisher(data){
    return await axios.patch(mainUrl + "/publisher/updatePublisher", data).then(res => res.data)
}


export async function requestDeleteUser(id, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };
    console.log(token)
    return await axios.delete(mainUrl + "/auth/deleteUser/" + id, config).then(res => res.data);
}

