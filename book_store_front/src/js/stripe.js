import { loadStripe } from '@stripe/stripe-js';
import axios from "axios";

const stripePromise = await loadStripe("pk_test_51OIGBKKfdlsNCGTnyxFs1IzyDJ1Wfe4TKOpDgeDyyubqHixilJu2an4WBdktNWgAUqfPMV6fw8eLNjf6QumdqC9X00g6whFvLS");

export const buyBooks = async (orderIdQuantityBooks) => {
    const session  = await axios.post(`http://localhost:5224/order/makeOrder`, orderIdQuantityBooks).then(res => res.data);

    // const stripe = await stripePromise;

    await stripePromise.redirectToCheckout({
        sessionId: session.sessionId
    });
};