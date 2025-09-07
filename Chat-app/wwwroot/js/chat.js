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
    const room = roomsCache.find(r => r.name === roomName);
    if (!room) {
        alert("Room does not exist. Please select a valid room.");
        return;
    }

    localStorage.setItem("chatUserName", userName);
    window.location.href = `/Room/${room.id}`;
});

async function loadRooms() {
    const rooms = await connection.invoke("GetRooms");
    updateRoomList(rooms);
}

function updateRoomList(rooms){
    roomsCache = rooms;

    const roomList = document.getElementById("roomList");
    roomList.innerHTML = ''; // clear existing list

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
