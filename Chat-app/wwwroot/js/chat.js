"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.start()
    .then(() => {
        loadRooms(); 
    })
    .catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("joinRoom").addEventListener("click", function () {
    const roomName = document.getElementById("roomName").value;
    const userName = document.getElementById("userName").value;

    if (!roomName) {
        alert("Select a Room");
        return;
    }

    if (!userName) {
        alert("Enter username");
        return;
    }

    connection.invoke("JoinRoom", userName, roomName);
    document.getElementById("homeScreen").style.display = "none";
    document.getElementById("chatScreen").style.display = "block";
    document.getElementById("roomTitle").innerHTML = roomName;
    document.getElementById("messageInput").focus();
});

document.getElementById("messageInput").addEventListener("keyup", function (event) {
    if (event.key === "Enter") {
        const message = document.getElementById("messageInput").value;
        const roomName = document.getElementById("roomName").value;
        if (message && roomName) {
            connection.invoke("SendMessageToRoom", roomName, message);
            document.getElementById("messageInput").value = '';
        }
    }
});

connection.on("ReceiveMessage", function (msg) {
    const messages = document.getElementById("messages");
    const user = `<span style="font-weight: bold">${msg.user}: </span>`;
    messages.innerHTML += `<p>${user}<span>${msg.content}</span></p>`;
});

connection.on("UserJoined", function (msg) {
    const messages = document.getElementById("messages");
    const user = `<span style="font-weight: bold">${msg} </span>`;
    messages.innerHTML += `<p style="color:grey">${user}has joined.</p>`;
});

connection.on("UserLeft", function (msg) {
    const messages = document.getElementById("messages");
    const user = `<span style="font-weight: bold">${msg} </span>`;
    messages.innerHTML += `<p style="color:grey">${user}has left.</p>`;
});


async function loadRooms()
{
    const rooms = await connection.invoke("GetRooms");
    const roomList = document.getElementById("roomList");
    rooms.forEach(room => {
        const li = document.createElement("li");
        li.textContent = room.name;
        li.style.cursor = "pointer";
        li.onclick = () => {
            document.getElementById("roomName").value = room.name;
        };
        roomList.appendChild(li);
    });
}