using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public enum EActionTypes
    {
        acceptEnterpriseJoinRequest,
        addAttachmentToCard,
        addChecklistToCard,
        addMemberToBoard,
        addMemberToCard,
        addMemberToOrganization,
        addOrganizationToEnterprise,
        addToEnterprisePluginWhitelist,
        addToOrganizationBoard,
        commentCard,
        convertToCardFromCheckItem,
        copyBoard,
        copyCard,
        copyCommentCard,
        createBoard,
        createCard,
        createList,
        createOrganization,
        deleteBoardInvitation,
        deleteCard,
        deleteOrganizationInvitation,
        disableEnterprisePluginWhitelist,
        disablePlugin,
        disablePowerUp,
        emailCard,
        enableEnterprisePluginWhitelist,
        enablePlugin,
        enablePowerUp,
        makeAdminOfBoard,
        makeNormalMemberOfBoard,
        makeNormalMemberOfOrganization,
        makeObserverOfBoard,
        memberJoinedTrello,
        moveCardFromBoard,
        moveCardToBoard,
        moveListFromBoard,
        moveListToBoard,
        removeChecklistFromCard,
        removeFromEnterprisePluginWhitelist,
        removeFromOrganizationBoard,
        removeMemberFromCard,
        removeOrganizationFromEnterprise,
        unconfirmedBoardInvitation,
        unconfirmedOrganizationInvitation,
        updateBoard,
        updateCard,
        updateCheckItemStateOnCard,
        updateChecklist,
        updateList,
        updateMember,
        updateOrganization
    }
}
