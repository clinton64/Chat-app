"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
let roomsCache = [];

connection.start()
    .then(() => {
        loadRooms();
    })
    .catch(function (err) {
        return console.error(err.toString());
    });

document.getElementById("joinRoom").addEventListener("click", function () {
    const roomName = document.getElementById("roomName").value.trim();
    const userName = document.getElementById("userName").value.trim();

    if (!roomName) {
        alert("Select a Room");
        return;
    }

    if (!userName) {
        alert("Enter username");
        return;
    }

    // validate room name against cached rooms
    const roomExists = roomsCache.some(r => r.name === roomName);
    if (!roomExists) {
        alert("Room does not exist. Please select a valid room.");
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
        const message = document.getElementById("messageInput").value.trim();
        const roomName = document.getElementById("roomName").value;
        if (message && roomName) {
            connection.invoke("SendMessageToRoom", roomName, message);
            document.getElementById("messageInput").value = '';
        }
    }
});

connection.on("ReceiveMessage", function (msg) {
    const messages = document.getElementById("messages");

    const p = document.createElement("p");
    const strong = document.createElement("strong");
    strong.textContent = msg.user + ": ";
    const span = document.createElement("span");
    span.textContent = msg.content;

    p.appendChild(strong);
    p.appendChild(span);
    messages.appendChild(p);

    messages.scrollTop = messages.scrollHeight; // Auto-scroll to the latest message
});

connection.on("UserJoined", function (user) {
    const messages = document.getElementById("messages");
    const p = document.createElement("p");
    p.style.color = "grey";
    p.textContent = user + " has joined.";
    messages.appendChild(p);
    messages.scrollTop = messages.scrollHeight; // Auto-scroll
});

connection.on("UserLeft", function (user) {
    const messages = document.getElementById("messages");
    const p = document.createElement("p");
    p.style.color = "grey";
    p.textContent = user + " has left.";
});


async function loadRooms() {
    const rooms = await connection.invoke("GetRooms");
    updateRoomList(rooms);
}

function updateRoomList(rooms){
    roomsCache = rooms;

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

// 🔹 Listen for server notifications
//connection.on("RoomCreated", function (room) {
//    roomsCache.push(room);
//    updateRoomList(roomsCache);
//});

//connection.on("RoomRemoved", function (roomName) {
//    roomsCache = roomsCache.filter(r => r.name != roomName);
//    updateRoomList(roomsCache);
//});