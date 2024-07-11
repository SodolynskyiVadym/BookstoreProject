import axios from "axios";
import router from "../../../router";

const mainUrl = "http://localhost:5224/auth";



export async function getUserByToken(token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.get(mainUrl + "/getUserByToken", config).then(res => res.data);
    } catch (error) {
        console.error("Error fetching user by token:", error);
        await router.push("/error");
    }
}


export async function getAllUsers() {
    try {
        return await axios.get(mainUrl + "/getAllUsers");
    } catch (error) {
        console.error("Error fetching all users:", error);
        await router.push("/error");
    }
}


export async function updatePassword(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.patch(mainUrl + "/updatePassword", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error updating user:", error);
        await router.push("/error");
    }
}


export async function login(data) {
    try {
        return await axios.post(mainUrl + "/login", data).then(res => res.data);
    } catch (error) {
        console.error("Error logging in:", error);
        await router.push("/error");
    }
}



export async function postRegistrationEmployeer(data, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.post(mainUrl + "/registerWorker", data, config).then(res => res.data);
    } catch (error) {
        console.error("Error registering worker:", error);
        await router.push("/error");
    }
}

export async function postRegistrationUser(data) {
    try {
        return await axios.post(mainUrl + "/registerUser", data,).then(res => res.data);
    } catch (error) {
        console.error("Error registering user:", error);
        await router.push("/error");
    }
}

export async function deleteUser(id, token) {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };

    try {
        return await axios.delete(mainUrl + "/deleteUser/" + id, config).then(res => res.data);
    } catch (error) {
        console.error("Error deleting user:", error);
        await router.push("/error");
    }
}