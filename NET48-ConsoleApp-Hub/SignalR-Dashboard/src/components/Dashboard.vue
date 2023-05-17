<template>
    <div>
        <h1>Server Status Dashboard</h1>
        <div v-for="server in servers" :key="server.name">
            <h2>{{ server.name }}</h2>
            <p>Status: {{  server.status }}</p>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

interface ServerStatus {
    name: string,
    status: string
};

export default defineComponent({
    data() {
        return {
            connection: null as unknown,
            servers: [] as Array<ServerStatus>,
        }
    },
    methods: {
        addServerStatus(serverName: string, status: string) {
            const server = this.servers.find(s => s.name === serverName);
            if(server) {
                server.status = status;
            } else {
                this.servers.push({ name: serverName, status: status } as ServerStatus);
            }
        }
    },
    created() {
// @ts-ignore
        this.connection = $.hubConnection("http://localhost:8080/signalr/statusHub");
// @ts-ignore
        const statusHubProxy = this.connection.createHubProxy('statusHub');

        statusHubProxy.on("sendStatus", (serverName: string, status: string) => {
            this.addServerStatus(serverName, status);
        });
// @ts-ignore
        this.connection.start()
            .done(() => console.log("Now Connected"))
// @ts-ignore
            .fail(err => console.log("Could not connect: " + err));

    },
    beforeDestroy() {
        if(this.connection) {
// @ts-ignore
            this.connection.stop();
        }
    }
});
</script>
