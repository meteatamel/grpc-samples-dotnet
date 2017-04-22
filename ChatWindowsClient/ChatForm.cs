// Copyright 2017, Google Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of Google Inc. nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
using Grpc.Core;
using System;
using System.Windows.Forms;
using Com.Example.Grpc.Chat;
using System.Threading;

namespace ChatWindowsClient
{
    public partial class ChatForm : Form
    {
        private const string Host = "localhost";
        private const int Port = 8080;
  
        private ChatService.ChatServiceClient _chatService;

        public ChatForm()
        {
            InitializeComponent();
            InitializeGrpc();
        }

        private void InitializeGrpc()
        {
            // Create a channel
            var channel = new Channel(Host + ":" + Port, ChannelCredentials.Insecure);

            // Create a client with the channel
            _chatService = new ChatService.ChatServiceClient(channel);
        }

        private async void ChatForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Open a connection to the server
                using (var call = _chatService.chat())
                {
                    while (await call.ResponseStream.MoveNext(CancellationToken.None))
                    {
                        var serverMessage = call.ResponseStream.Current;
                        var otherClientMessage = serverMessage.Message;
                        // TODO: Display the message
                        var displayMessage = string.Format("%s:%s\n", otherClientMessage.From, otherClientMessage.Message);
                        chatTextBox.Text += displayMessage;
                    }
                }
            }
            catch (RpcException)
            {
                throw;
            }
        }

        private async void sendButton_Click(object sender, EventArgs e)
        {
            // Create a message
            var message = new ChatMessage
            {
                From = nameTextBox.Text,
                Message = messageTextBox.Text
            };

            try
            {
                // Send the message
                using (var call = _chatService.chat())
                {
                    await call.RequestStream.WriteAsync(message);
                    await call.RequestStream.CompleteAsync();
                }
            }
            catch (RpcException)
            {
                throw;
            }
        }
    }
}
