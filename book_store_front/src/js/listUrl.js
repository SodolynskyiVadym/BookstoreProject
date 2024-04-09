import axios from "axios";

const mainUrl = "http://localhost:5224";



export async function getUserByToken(token) {
  const config = {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  };

  return await axios.get(mainUrl + "/auth/getUserByToken", config).then(res => res.data);
}



export async function getBooks(){
    return await axios.get(mainUrl + "/book/getAllBooks").then(res => res.data);
}

export async function getReviews(id){
    return await axios.get(mainUrl + "/review/getReviews/" + id).then(res => res.data);
}


export async function getPublisher(id){
    return await axios.get(mainUrl + "/publisher/getPublisher/" + id).then(res => res.data);
}

export async function getReviewUserBook(bookId, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };
    return await axios.get(mainUrl + "/review/GetReview/" + bookId, config).then(res => res.data);
}



export async function getBook(id){
    return await axios.get(mainUrl + "/book/getBook/" + id).then(res => res.data);
}

export async function getSomeBook(arrayBooks){
    const strBooksId = arrayBooks.join(",");
    return await axios.get(mainUrl + "/book/getSomeBooks?ids=" +  strBooksId).then(res => res.data);
}

export async function getAuthorBooks(id){
    return await axios.get(mainUrl + "/author/getAuthorBooks/" + id).then(res => res.data);
}


export async function getPublisherBooks(id){
    return await axios.get(mainUrl + "/publisher/getPublisherBooks/" + id).then(res => res.data);
}


export async function getAuthor(id){
    return await axios.get(mainUrl + "/author/getAuthor/" + id).then(res => res.data);
}


export async function getAllAuthors(){
    return await axios.get(mainUrl + "/author/getAllAuthors").then(res => res.data);
}


export async function getAllPublishers(){
    return await axios.get(mainUrl + "/publisher/getAllPublishers").then(res => res.data);
}


export async function getAllUsers(){
    return await axios.get(mainUrl + "/auth/getAllUsers").then(res => res.data);
}


export async function postCreateBook(data, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };

    return await axios.post(mainUrl + "/book/createBook", data, config).then(res => res.data)
}

export async function postCreateReview(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    return await axios.post(mainUrl + "/review/createReview", data, config).then(res => res.data)
}



export async function postCreateAuthor(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };
    return await axios.post(mainUrl + "/author/createAuthor", data, config).then(res => res.data)
}


export async function postCreatePublisher(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };
    return await axios.post(mainUrl + "/publisher/createPublisher", data, config).then(res => res.data)
}


export async function patchUpdateUser(data, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };
    return await axios.patch(mainUrl + "/auth/updateUser", data, config).then(res => res.data)
}

export async function postRegistration(data){
    return await axios.post(mainUrl + "/auth/registerUser", data).then(res => res.data)
}


export async function postLogin(data){
    return await axios.post(mainUrl + "/auth/login", data).then(res => res.data)
}


export async function postRegistrationWorker(data, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };
    return await axios.post(mainUrl + "/auth/registerWorker", data, config).then(res => res.data);
}


export async function postCreateBookImage(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    return await axios.post(mainUrl + "/book/createImageBook", data, config).then(res => res.data);
}


export async function postCreatePublisherImage(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    return await axios.post(mainUrl + "/publisher/createImagePublisher", data, config).then(res => res.data);
}


export async function postCreateAuthorImage(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    return await axios.post(mainUrl + "/author/createImageAuthor", data, config).then(res => res.data);
}


export async function patchUpdateBook(data){
    return await axios.patch(mainUrl + "/book/updateBook", data).then(res => res.data)
}


export async function patchUpdateAuthor(data){
    return await axios.patch(mainUrl + "/author/updateAuthor", data).then(res => res.data)
}


export async function patchUpdatePublisher(data){
    return await axios.patch(mainUrl + "/publisher/updatePublisher", data).then(res => res.data)
}

export async function patchUpdateReview(data, token){
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    return await axios.patch(mainUrl + "/review/updateReview", data, config).then(res => res.data);
}


export async function deleteUser(id, token){
    const config = {
        headers: {
          'Authorization': `Bearer ${token}`
        }
    };
    return await axios.delete(mainUrl + "/auth/deleteUser/" + id, config).then(res => res.data);
}

