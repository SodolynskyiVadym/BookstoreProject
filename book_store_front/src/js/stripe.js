import { loadStripe } from '@stripe/stripe-js';
import axios from "axios";


let stripePromise;

const initializeStripe = async () => {
    stripePromise = await loadStripe("pk_test_51OIGBKKfdlsNCGTnyxFs1IzyDJ1Wfe4TKOpDgeDyyubqHixilJu2an4WBdktNWgAUqfPMV6fw8eLNjf6QumdqC9X00g6whFvLS");
}

initializeStripe();

export const buyBooks = async (order, token) => {
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    }
    const id  = await axios.post(`http://localhost:5224/order/makeOrder`, order, config).then(res => res.data);

    const stripe = await stripePromise;

    await stripe.redirectToCheckout({
        sessionId: id
    });
};