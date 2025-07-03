| 🟢 Milestone | Feature Area                   | Summary                             |
| ------------ | ------------------------------ | ----------------------------------- |
|  M1         | Auth                           | Register, Login, JWT, `[Authorize]` |
|  M2        | Conversations & Participants   | Users create 1-on-1 or group chats  |
|  M3        | Messaging                      | Users send and retrieve messages    |
|  M4        | Search & User Discovery        | Search users and conversations      |
|  M5        | Realtime (SignalR)             | Live messaging                      |
|  M7        | Attachments                    | File uploads                        |
|  M8         | Admin tools & Roles (optional) | Kick, promote, etc.                 |


| 🔢 #      | 🧩 Milestone             | 🔧 Focus Area                         | ✅ Goal                                               |
|-----------| ------------------------ | ------------------------------------- | ---------------------------------------------------- |
| **✅ M1**  | 🔐 Auth & Identity       | Register, Login, JWT, Auth Middleware | Users can register, log in, and hit protected routes |
| **🟡 M2** | 💬 Conversations         | 1-on-1 and Group Chat Setup           | Users can create 1-on-1 or group chats               |
| **🔵 M3** | ✉️ Messaging             | Send, fetch, and paginate messages    | Users can send/read messages in conversations        |
| **🟣 M4** | 🧑‍🤝‍🧑 Participants    | Add/remove users from groups          | Users can manage group members                       |
| **🟠 M5** | 🔎 Search & Discovery    | Search users and chats                | Search people to start chats or find conversations   |
| **🔴 M6** | 🌐 Realtime (SignalR)    | Live messaging support                | Users receive messages instantly                     |
| **🟤 M7** | 📁 Attachments           | File/image sending                    | Users can send/receive files in chat                 |
| **⚪ M8**  | 📊 Presence & Status     | Online status, typing...              | Show who's online, typing, etc.                      |
|  M6        | Notifications  | push                 |
| **M9**    | 🛠️ Admin Tools          | Group roles, kicking users            | Group owners can manage participants                 |
| **M10**   | 🧪 Testing & CI/CD       | Unit + integration tests, CI pipeline | Automated tests + deployment ready                   |
| **M11**   | 🔐 Security Enhancements | Rate limiting, token rotation         | Harden the system against abuse                      |
| **M12**   | 📜 Documentation         | Swagger + Readme                      | Devs can explore the API easily                      |

# Reactions
# Message indicators:
seen/read,
sent => received => sent, 
typing indicator(in future)
edited:
deleted
✅ 6. Pinned Messages
Reaction summary/count


🧠 Optional Extras (Advanced or for enterprise-like apps):
Feature	Why it’s useful
IsFlagged	For reporting abusive content
MessagePriority	Priority support (like Slack’s /important)
ThreadId	For supporting threaded replies
IsSystemMessage	For bot or system-generated messages (e.g. "User joined")
IsSilent	Messages that don't send notifications
Mark for later
Remember in ... (feature)


# Admin tool
   Delete any message.
   View all users.
   Ban user from chat.
   

# message status - Not the best solution, should mark each message as read per message