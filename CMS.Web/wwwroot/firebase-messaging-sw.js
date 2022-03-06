importScripts('https://www.gstatic.com/firebasejs/9.1.1/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/9.1.1/firebase-messaging.js');

var config = {
    apiKey: "AIzaSyCmCWruVV7F0y1cCwE_s93fXVJIcwRBnQ4",
    authDomain: "cmsweb-b1aa8.firebaseapp.com",
    projectId: "cmsweb-b1aa8",
    storageBucket: "cmsweb-b1aa8.appspot.com",
    messagingSenderId: "1020282346736",
    appId: "1:1020282346736:web:d42de194e7442f9c91f635",
    measurementId: "G-H7R0REYVQR"
};

firebase.initializeApp(config);

const messaging = firebase.messaging();

messaging.setBackgroundMessageHandler(function(payload) {
    //// Customize notification here
    var notificationTitle = 'My Titile';
    var notificationOptions = {
        body: payload.data.body
    };

    return self.registration.showNotification(notificationTitle,
        notificationOptions);
});